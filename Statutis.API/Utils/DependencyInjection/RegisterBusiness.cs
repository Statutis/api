using Statutis.Business;
using Statutis.Business.History;
using Statutis.Core.Interfaces.Business;
using Statutis.Core.Interfaces.Business.History;
using Statutis.Core.Interfaces.Business.Service;

namespace Statutis.API.Utils.DependencyInjection;

public static class RegisterBusiness
{
	public static void AddBusiness(this IServiceCollection service)
	{
		service.AddScoped<IPasswordHash, PasswordHash>();
		service.AddScoped<IServiceTypeService, ServiceTypeService>();
		service.AddScoped<IServiceService, ServiceService>();
		service.AddScoped<ITeamService, TeamService>();
		service.AddScoped<IHistoryEntryService, HistoryEntryService>();
	}
}