using Statutis.Business;
using Statutis.Core.Interfaces.Business;
using Statutis.Core.Interfaces.Business.Service;

namespace Statutis.API.Utils.DependencyInjection;

public static class RegisterBusiness
{
	public static void AddBusiness(this IServiceCollection service)
	{
		service.AddScoped<IPasswordHash, PasswordHash>();
		service.AddScoped<IServiceTypeService, ServiceTypeService>();
		service.AddScoped<ITeamService, TeamService>();
	}
}