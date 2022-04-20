using Microsoft.EntityFrameworkCore;
using Statutis.API.Utils.DependencyInjection;
using Statutis.Cron.ServiceChecker;
using Statutis.DbRepository;

var builder = Host.CreateDefaultBuilder(args);



IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((hostContext, services) =>
	{
		IConfiguration configuration = hostContext.Configuration;
		string hostname = configuration.GetConnectionString("hostname");
		string port = configuration.GetConnectionString("port");
		string username = configuration.GetConnectionString("username");
		string password = configuration.GetConnectionString("password");
		string database = configuration.GetConnectionString("database");
		
		services.AddDbContext<StatutisContext>(opt => opt.UseNpgsql(
			@"Host=" + hostname + ";Username=" + username + ";Password=" + password + ";Database=" + database + ""));

		services.AddHttpClient();
		
		services.AddDbRepositories();
		services.AddBusiness();
		
		services.AddHostedService<PingCheckerWorker>();
		services.AddHostedService<HttpCheckerService>();
	})
	.Build();

await host.RunAsync();