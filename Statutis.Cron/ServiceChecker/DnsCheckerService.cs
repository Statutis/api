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

					var lookup = new LookupClient();

					var result = await lookup.QueryAsync(_service.Host, queryType);

					if (!result.HasError && result.Answers.AddressRecords().Any(x => x.Address.ToString() == _service.Result))
					{
						entry.State = HistoryState.Online;
					}
					else
					{
						entry.State = HistoryState.Unreachable;
						entry.message = result.ErrorMessage;
					}


					await historyService.Add(entry);
					_logger.LogInformation("Status du service {1}({2}) : {0}", _service.Name, _service.ServiceId, entry.State.ToString());
				}


			}


			await Task.Delay(waitingSeconds * 1000, stoppingToken);
		}
	}
}