using Microsoft.Extensions.DependencyInjection;
using Statutis.Core.Interfaces.DbRepository;
using Statutis.Core.Interfaces.DbRepository.History;
using Statutis.Core.Interfaces.DbRepository.Service;
using Statutis.DbRepository.Repository;
using Statutis.DbRepository.Repository.History;
using Statutis.DbRepository.Repository.Service;

namespace Statutis.DbRepository;

public static class RegisterDbRepository
{
	public static void AddDbRepositories(this IServiceCollection service)
	{
		service.AddScoped<IServiceTypeRepository, ServiceTypeRepository>();
		service.AddScoped<IServiceRepository, ServiceRepository>();
		service.AddScoped<ITeamRepository, TeamRepository>();
		service.AddScoped<IUserRepository, UserRepository>();
		service.AddScoped<IDnsServiceRepository, DnsServiceRepository>();
		service.AddScoped<IGroupRepository, GroupRepository>();
		service.AddScoped<IHttpServiceRepository, HttpServiceRepository>();
		service.AddScoped<IPingServiceRepository, PingServiceRepository>();
		service.AddScoped<ISshServiceRepository, SshServiceRepository>();
		service.AddScoped<IHistoryEntryRepository, HistoryEntryRepository>();
	}
}