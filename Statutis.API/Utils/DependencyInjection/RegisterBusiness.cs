using Statutis.Business;
using Statutis.Core.Interfaces.Business;
using Statutis.Core.Interfaces.Business.Service;

namespace Statutis.API.Utils.DependencyInjection;

public static class RegisterBusiness
{
	public static void AddBusiness(this IServiceCollection service)
	{
		service.AddSingleton<IPasswordHash, PasswordHash>();
		service.AddSingleton<IServiceTypeService, ServiceTypeService>();
	}
}