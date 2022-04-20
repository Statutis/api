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
				_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

				IServiceService serviceService = scope.ServiceProvider.GetRequiredService<IServiceService>();
				IHistoryEntryService historyService = scope.ServiceProvider.GetRequiredService<IHistoryEntryService>();
				List<PingService> pingServices = await serviceService.GetAll<PingService>();


				foreach (PingService pingService in pingServices)
				{

					HistoryEntry entry = new HistoryEntry(pingService, DateTime.UtcNow);

					Ping ping = new Ping();
					PingReply reply = ping.Send(pingService.Host);
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

				}


			}


			await Task.Delay(waitingSeconds * 1000, stoppingToken);
		}
	}
}