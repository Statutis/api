using System.Net;
using DnsClient;
using Statutis.Core.Interfaces.Business.History;
using Statutis.Core.Interfaces.Business.Service;
using Statutis.Entity.History;
using Statutis.Entity.Service.Check;

namespace Statutis.Cron.ServiceChecker;

public class DnsCheckerService : BackgroundService
{
	private readonly ILogger<DnsCheckerService> _logger;
	private readonly IServiceProvider _serviceProvider;
	private readonly IConfiguration _configuration;

	public DnsCheckerService(ILogger<DnsCheckerService> logger, IServiceProvider serviceProvider, IConfiguration configuration)
	{
		_logger = logger;
		_serviceProvider = serviceProvider;
		_configuration = configuration;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		int waitingSeconds = _configuration.GetSection("Application").GetSection("secondsBetweenCheck").Get<int>();

		while (!stoppingToken.IsCancellationRequested)
		{

			using (IServiceScope scope = _serviceProvider.CreateScope())
			{
				IServiceService serviceService = scope.ServiceProvider.GetRequiredService<IServiceService>();
				IHistoryEntryService historyService = scope.ServiceProvider.GetRequiredService<IHistoryEntryService>();
				List<DnsService> dnsServices = await serviceService.GetAll<DnsService>();
				;
				_logger.LogInformation("Lancement des vérifications DNS (" + dnsServices.Count + " services} :");

				foreach (DnsService _service in dnsServices)
				{

					_logger.LogInformation("Vérifcation du service {0} ({1})", _service.Name, _service.ServiceId);
					HistoryEntry entry = new HistoryEntry(_service, DateTime.UtcNow, HistoryState.Error);

					QueryType queryType;

					if (!QueryType.TryParse(_service.Type, out queryType))
					{
						_logger.LogError("Impossible de trouver le type de query {0} pour le service {1}.", _service.Type, _service.ServiceId);
						continue;
					}

					try
					{
						var lookup = new LookupClient();	var result = await lookup.QueryAsync(_service.Host, queryType);

						if (!result.HasError )
					{
						if (isValid(_service.Result, result))
							entry.State = HistoryState.Online;else
							entry.State = HistoryState.Error;
						}
						else
						{
							entry.State = HistoryState.Unreachable;
							entry.message = result.ErrorMessage;
						}

					}
					catch (Exception e)
					{
						Console.WriteLine(e);
						entry.State = HistoryState.Unreachable;
						entry.message = e.Message;
					}
					

				

					await historyService.Add(entry);
					_logger.LogInformation("Status du service {1}({2}) : {0}", _service.Name, _service.ServiceId, entry.State.ToString());
				}


			}


			await Task.Delay(waitingSeconds * 1000, stoppingToken);
		}
	}

	private bool isValid(string expected, IDnsQueryResponse result)
	{
		expected = expected.ToLower().Trim();

		return result.Answers.MxRecords().Any(x=>isStringValid(x.Exchange, expected))
		       || result.Answers.AddressRecords().Any(x => isStringValid(x.Address.ToString(), expected))
		       || result.Answers.PtrRecords().Any(x => isStringValid(x.PtrDomainName.Value, expected))
		       || result.Answers.CnameRecords().Any(x => isStringValid(x.CanonicalName.Value, expected))
		       || result.Answers.AaaaRecords().Any(x => isStringValid(x.Address.ToString(), expected))
		       || result.Answers.NsRecords().Any(x => isStringValid(x.NSDName.Value, expected))
		       || result.Answers.SrvRecords().Any(x => isStringValid(x.Target.Value, expected))
		       || result.Answers.ARecords().Any(x => isStringValid(x.Address.ToString(), expected))
		       || result.Answers.TxtRecords().Any(x => x.Text.Any(y=>isStringValid(y, expected)))
		       ;
	}

	private bool isStringValid(string value, string expected) => value.Trim().ToLower() == expected || value.Trim().ToLower() == expected + ".";
}