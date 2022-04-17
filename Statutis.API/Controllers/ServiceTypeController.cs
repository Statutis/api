using Microsoft.AspNetCore.Mvc;
using Statutis.API.Models;
using Statutis.Core.Interfaces.Business.Service;

namespace Statutis.API.Controllers
{
	[Route("api/services/types/")]
	[ApiController]
	public class ServiceTypeController : Controller
	{

		private IServiceTypeService _serviceType;

		public ServiceTypeController(IServiceTypeService serviceType)
		{
			_serviceType = serviceType;
		}

		[HttpGet, Route("")]
		public async Task<IActionResult> GetAll()
		{
			return Ok((await _serviceType.GetAll()).Select(x => new ServiceTypeModel(x)));
		}
	}
}