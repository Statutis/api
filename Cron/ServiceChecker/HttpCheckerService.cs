using System.Net.NetworkInformation;
using Statutis.Core.Interfaces.Business.History;
using Statutis.Core.Interfaces.Business.Service;
using Statutis.Entity.History;
using Statutis.Entity.Service.Check;

namespace Cron;

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
				_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

				IServiceService serviceService = scope.ServiceProvider.GetRequiredService<IServiceService>();
				IHistoryEntryService historyService = scope.ServiceProvider.GetRequiredService<IHistoryEntryService>();
				List<HttpService> pingServices = await serviceService.GetAll<HttpService>();


				foreach (HttpService _service in pingServices)
				{

					HistoryEntry entry = new HistoryEntry(_service, DateTime.UtcNow, HistoryState.Error);

					var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _service.Host);
					var httpClient = _httpClientFactory.CreateClient();
					
					var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, stoppingToken);

					if (_service.Code == null || (int)httpResponseMessage.StatusCode == _service.Code)
					{
						entry.State = HistoryState.Online;
					}

					await historyService.Add(entry);

				}


			}


			await Task.Delay(waitingSeconds * 1000, stoppingToken);
		}
	}
}