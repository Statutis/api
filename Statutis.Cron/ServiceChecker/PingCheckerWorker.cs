using System.Net.NetworkInformation;
using Statutis.Core.Interfaces.Business.History;
using Statutis.Core.Interfaces.Business.Service;
using Statutis.Entity.History;
using Statutis.Entity.Service.Check;

namespace Statutis.Cron.ServiceChecker;

public class PingCheckerWorker : BackgroundService
{
	private readonly ILogger<PingCheckerWorker> _logger;
	private readonly IServiceProvider _serviceProvider;
	private readonly IConfiguration _configuration;

	public PingCheckerWorker(ILogger<PingCheckerWorker> logger, IServiceProvider serviceProvider, IConfiguration configuration)
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
				List<PingService> pingServices = await serviceService.GetAll<PingService>();
				
				_logger.LogInformation("Lancement des vérifications Ping (" + pingServices.Count + " services} :");

				foreach (PingService _service in pingServices)
				{
					_logger.LogInformation("Vérifcation du service {0} ({1})", _service.Name, _service.ServiceId);
					HistoryEntry entry = new HistoryEntry(_service, DateTime.UtcNow);

					Ping ping = new Ping();
					PingReply reply = ping.Send(_service.Host);
					switch (reply.Status)
					{
						case IPStatus.Success:
							entry.State = HistoryState.Online;
							break;
						case IPStatus.Unknown:
							entry.State = HistoryState.Unknown;
							break;
						case IPStatus.BadDestination or IPStatus.BadRoute or IPStatus.TimedOut or IPStatus.TimeExceeded:
							entry.State = HistoryState.Unreachable;
							break;
						default:
							entry.State = HistoryState.Error;
							break;
					}


					await historyService.Add(entry);
					_logger.LogInformation("Status du service {1}({2}) : {0}", _service.Name, _service.ServiceId, entry.State.ToString());
				}


			}


			await Task.Delay(waitingSeconds * 1000, stoppingToken);
		}
	}
}