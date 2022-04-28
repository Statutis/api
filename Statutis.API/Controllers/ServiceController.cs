using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Statutis.API.Form;
using Statutis.API.Models;
using Statutis.Core.Interfaces.Business;
using Statutis.Core.Interfaces.Business.History;
using Statutis.Core.Interfaces.Business.Service;
using Statutis.Entity;
using Statutis.Entity.History;
using Statutis.Entity.Service;
using Statutis.Entity.Service.Check;

namespace Statutis.API.Controllers;

/// <summary>
/// Controlleur sur les services
/// </summary>
[Route("api/services")]
[ApiController]
public class ServiceController : Controller
{
	private IHistoryEntryService _historyEntryService;
	private IServiceService _serviceService;
	private readonly IUserService _userService;
	private readonly IServiceTypeService _serviceTypeService;

	/// <summary>
	/// Constructeur
	/// </summary>
	/// <param name="historyEntryService"></param>
	/// <param name="service"></param>
	/// <param name="userService"></param>
	/// <param name="serviceTypeService"></param>
	public ServiceController(IHistoryEntryService historyEntryService,
		IServiceService service,
		IUserService userService,
		IServiceTypeService serviceTypeService)
	{
		_historyEntryService = historyEntryService;
		_serviceService = service;
		_userService = userService;
		_serviceTypeService = serviceTypeService;
	}

	/// <summary>
	/// Ajout d'un service DNS
	/// </summary>
	/// <param name="form">Informations sur le service</param>
	/// <returns>Un service DNS</returns>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	/// <response code="404">Si la référence vers le groupe ou le type de service ne sont pas trouvés.</response>
	/// <response code="403">Si l'utilisateur courant n'a pas les droits sur le groupe cible.</response>
	[HttpPost("dns")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DnsServiceModel))]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Add([FromBody] DnsForm form)
	{
		if ((HttpContext.User.Identity?.IsAuthenticated ?? false) == false)
			return Forbid();

		User? user = await _userService.GetUserAsync(User);
		if (user == null)
			return Unauthorized();

		bool canParse = Guid.TryParse(form.GroupRef.Split("/").Last(), out Guid groupId);

		if (!canParse)
			return NotFound();

		if (!(await _userService.isUserInGroup(user, groupId)))
		{
			return Forbid();
		}

		string serviceTypeName = form.ServiceTypeRef.Split("/").Last();

		if (await _serviceTypeService.Get(serviceTypeName) == null)
			return NotFound();

		DnsService dnsService = new DnsService()
		{
			Description = form.Description,
			GroupId = groupId,
			Host = form.Host,
			Name = form.Name,
			Result = form.Result,
			Type = form.Type,
			ServiceTypeName = serviceTypeName
		};
		Service service = await _serviceService.Insert(dnsService);
		return Ok(new DnsServiceModel(dnsService, HistoryState.Unknown, Url));
	}

	/// <summary>
	/// Récupération des modes de vérifications
	/// </summary>
	/// <returns>Liste des modes de vérifications</returns>
	[HttpGet, Route("checks")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<String>))]
	public Task<IActionResult> GetCheckType()
	{
		return Task.FromResult<IActionResult>(Ok(new List<String>()
			{ DnsService.CheckType, HttpService.CheckType, PingService.CheckType }));
	}

	/// <summary>
	/// Récupération de l'état global
	/// </summary>
	/// <returns></returns>
	[HttpGet, Route("state")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MainStateModel))]
	public async Task<IActionResult> GetAll()
	{
		var res = await _historyEntryService.GetMainState();
		return Ok(new MainStateModel() { State = res.Item1, LastUpdate = res.Item2 });
	}

	/// <summary>
	/// Récupération d'un service
	/// </summary>
	/// <param name="guid">Identifiant du service cible</param>
	/// <returns>Un service</returns>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	/// <response code="404">Si le service n'est pas trouvé.</response>
	/// <response code="403">Si l'utilisateur courant n'a pas les droits sur le groupe cible.</response>
	[HttpGet, Route("{guid}"), Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceModel))]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Get(Guid guid)
	{
		var res = await _serviceService.Get(guid);
		if (res == null)
			return NotFound();
		
		var user = await _userService.GetUserAsync(User);
		if (user == null || _userService.IsUserInTeam(user, res.Group.Teams))
			return Forbid();

		return Ok(new ServiceModel(res, res.HistoryEntries.Last(), Url));
	}
}