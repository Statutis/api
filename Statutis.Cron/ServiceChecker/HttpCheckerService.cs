using Statutis.Core.Interfaces.Business.History;
using Statutis.Core.Interfaces.Business.Service;
using Statutis.Entity.History;
using Statutis.Entity.Service.Check;

namespace Statutis.Cron.ServiceChecker;

public class HttpCheckerService : BackgroundService
{
	private readonly ILogger<HttpCheckerService> _logger;
	private readonly IServiceProvider _serviceProvider;
	private readonly IConfiguration _configuration;
	private readonly IHttpClientFactory _httpClientFactory;

	public HttpCheckerService(ILogger<HttpCheckerService> logger, IServiceProvider serviceProvider, IConfiguration configuration, IHttpClientFactory httpClientFactory)
	{
		_logger = logger;
		_serviceProvider = serviceProvider;
		_configuration = configuration;
		_httpClientFactory = httpClientFactory;
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
				List<HttpService> httpServices = await serviceService.GetAll<HttpService>();

				_logger.LogInformation("Lancement des vérifications HTTP (" + httpServices.Count + " services} :");
				
				foreach (HttpService _service in httpServices)
				{
					_logger.LogInformation("\tVérifcation du service {0} ({1})", _service.Name, _service.ServiceId);
					
					HistoryEntry entry = new HistoryEntry(_service, DateTime.UtcNow, HistoryState.Error);

					var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _service.Host);
					var httpClient = _httpClientFactory.CreateClient();
					
					var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, stoppingToken);

					if (_service.Code == null || (int)httpResponseMessage.StatusCode == _service.Code)
					{
						entry.State = HistoryState.Online;
					}

					await historyService.Add(entry);
					_logger.LogInformation("Status du service {1}({2}) : {0}", _service.Name, _service.ServiceId, entry.State.ToString());

				}


			}


			await Task.Delay(waitingSeconds * 1000, stoppingToken);
		}
	}
}