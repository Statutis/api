using Microsoft.Extensions.DependencyInjection;
using Statutis.Business;
using Statutis.Business.History;
using Statutis.Core.Interfaces.Business;
using Statutis.Core.Interfaces.Business.History;
using Statutis.Core.Interfaces.Business.Service;

namespace Statutis.Business;

public static class RegisterBusiness
{
	public static void AddBusiness(this IServiceCollection service)
	{
		service.AddScoped<IPasswordHash, PasswordHash>();
		service.AddScoped<ITeamService, TeamService>();
		service.AddScoped<IGroupService, GroupService>();
		service.AddScoped<IServiceTypeService, ServiceTypeService>();
		service.AddScoped<IServiceService, ServiceService>();
		service.AddScoped<IHistoryEntryService, HistoryEntryService>();
		service.AddScoped<IUserService, UserService>();
		service.AddScoped<IAuthService, AuthService>();
	}
}