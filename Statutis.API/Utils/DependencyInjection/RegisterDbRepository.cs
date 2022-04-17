using Statutis.Core.Interfaces.DbRepository.Service;
using Statutis.DbRepository.Repository.Service;

namespace Statutis.API.Utils.DependencyInjection;

public static class RegisterDbRepository
{
	public static void AddDbRepositories(this IServiceCollection service)
	{
		service.AddSingleton<IServiceRepository, ServiceRepository>();
	}
}