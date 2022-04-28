using Microsoft.AspNetCore.Mvc;
using Statutis.API.Models;
using Statutis.Core.Interfaces.Business.Service;
using Statutis.Entity.Service;

namespace Statutis.API.Controllers;

/// <summary>
/// Controleur sur les types de service
/// </summary>
[Tags("Type de service")]
[Route("api/services/types/")]
[ApiController]
public class ServiceTypeController : Controller
{

	private IServiceTypeService _serviceType;

	/// <summary>
	/// Constructeur
	/// </summary>
	/// <param name="serviceType"></param>
	public ServiceTypeController(IServiceTypeService serviceType)
	{
		_serviceType = serviceType;
	}

	/// <summary>
	/// Liste de tous les type de services
	/// </summary>
	/// <returns>Types de services</returns>
	[HttpGet, Route("")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ServiceTypeModel>))]
	public async Task<IActionResult> GetAll()
	{
		return Ok((await _serviceType.GetAll()).Select(x => new ServiceTypeModel(x, this.Url)));
	}

	/// <summary>
	/// Récupération d'un type de service spécifique
	/// </summary>
	/// <param name="name">Identifiant du type de service cible.</param>
	/// <returns>Type de services</returns>
	/// <response code="404">Si le type de service visé n'existe pas.</response>
	[HttpGet, Route("{name}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceTypeModel))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Get(string name)
	{
		ServiceType? type = await _serviceType.Get(name);
		if (type == null)
			return NotFound();
		return Ok(new ServiceTypeModel(type, this.Url));
	}
}