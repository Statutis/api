using System.Text.Encodings.Web;
using System.Web;
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
/// Controleur sur les services
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
	/// Ajout d'un service avec un mode de vérification par DNS
	/// </summary>
	/// <param name="form">Informations sur ce nouveau service</param>
	/// <returns>Le nouveau service avec un mode de vérification par DNS</returns>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	/// <response code="403">Si l'utilisateur courant n'a pas les droits sur le groupe cible.</response>
	[HttpPost("dns")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DnsServiceModel))]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> AddDns([FromBody] DnsForm form)
	{
		form.ServiceTypeRef = Uri.UnescapeDataString(form.ServiceTypeRef);
        bool status = CanAddService(form, out Guid groupGuid, out string serviceTypeName);
		if (!status)
			return Forbid();

		DnsService dnsService = new DnsService()
		{
			Description = form.Description,
			GroupId = groupGuid,
			Name = form.Name,
			Host = form.Host,
			ServiceTypeName = serviceTypeName,
			//Specific to dns
			Type = form.Type,
			Result = form.Result,

		};
		Service service = await _serviceService.Insert(dnsService);
		return Ok(new DnsServiceModel(dnsService, HistoryState.Unknown, Url));
	}

	

	/// <summary>
	/// Ajout d'un service avec un mode de vérification par HTTP
	/// </summary>
	/// <param name="form">Informations sur ce nouveau service</param>
	/// <returns>Le nouveau service avec un mode de vérification par HTTP</returns>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	/// <response code="403">Si l'utilisateur courant n'a pas les droits sur le groupe cible.</response>
	[HttpPost("http"), Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HttpServiceModel))]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> AddHttpService([FromBody] HttpForm form)
	{
		form.ServiceTypeRef = Uri.UnescapeDataString(form.ServiceTypeRef);
		bool status = CanAddService(form, out Guid groupGuid, out string serviceTypeName);
		if (!status)
			return Forbid();

		HttpService httpService = new HttpService()
		{
			Description = form.Description,
			GroupId = groupGuid,
			Name = form.Name,
			Host = form.Host,
			ServiceTypeName = serviceTypeName,
			//Specific to HTTP
			Code = form.Code,
			RedirectUrl = form.RedirectUrl
		};

		Service service = await _serviceService.Insert(httpService);
		return Ok(new HttpServiceModel(httpService, HistoryState.Unknown, Url));
	}

	/// <summary>
	/// Ajout d'un service avec un mode de vérification par PING
	/// </summary>
	/// <param name="form">Informations sur ce nouveau service</param>
	/// <returns>Le nouveau service avec un mode de vérification par PING</returns>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	/// <response code="403">Si l'utilisateur courant n'a pas les droits sur le groupe cible.</response>
	[HttpPost, Route("ping"), Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PingServiceModel))]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> AddPingService([FromBody] PingForm form)
	{
		form.ServiceTypeRef = Uri.UnescapeDataString(form.ServiceTypeRef);
        bool status = CanAddService(form, out Guid groupGuid, out string serviceTypeName);
		if (!status)
			return Forbid();

		PingService pingService = new PingService()
		{
			Description = form.Description,
			GroupId = groupGuid,
			Name = form.Name,
			Host = form.Host,
			ServiceTypeName = serviceTypeName,
		};

		Service service = await _serviceService.Insert(pingService);
		return Ok(new PingServiceModel(pingService, HistoryState.Unknown, Url));
	}
	
	
	/// <summary>
	/// Ajout d'un service avec un mode de vérification par AtlassianStatusPage
	/// </summary>
	/// <param name="form">Informations sur ce nouveau service</param>
	/// <returns>Le nouveau service avec un mode de vérification par AtlassianStatusPage</returns>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	/// <response code="403">Si l'utilisateur courant n'a pas les droits sur le groupe cible.</response>
	[HttpPost, Route("atlassian_status_page"), Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AtlassianStatusPageModel))]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> AddAtlassianStatusPageService([FromBody] AtlassianStatusPageForm form)
	{
		bool status = CanAddService(form, out Guid groupGuid, out string serviceTypeName);
		if (!status)
			return Forbid();

		AtlassianStatusPageService atlassianService = new AtlassianStatusPageService()
		{
			Description = form.Description,
			GroupId = groupGuid,
			Name = form.Name,
			Host = form.Host,
			ServiceTypeName = serviceTypeName,
		};

		Service service = await _serviceService.Insert(atlassianService);
		return Ok(new AtlassianStatusPageModel(atlassianService, HistoryState.Unknown, Url));
	}

	/// <summary>
	/// Récupération des modes de vérifications
	/// </summary>
	/// <returns>Liste de modes de vérifications</returns>
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


	/// <summary>
	/// Vérification s'il est possible d'ajouter un utilisateur
	/// </summary>
	/// <param name="form">Formulaire du service</param>
	/// <param name="groupGuid">Groupe cible</param>
	/// <param name="serviceTypeName">Type de service cible</param>
	/// <returns></returns>
	private bool CanAddService(ServiceForm form, out Guid groupGuid, out string serviceTypeName)
	{
		groupGuid = new Guid();
		serviceTypeName = "";

		if ((HttpContext.User.Identity?.IsAuthenticated ?? false) == false)
			return false;

		User? user = _userService.GetUserAsync(User).Result;
		if (user == null)
			return false;

		bool canParse = Guid.TryParse(form.GroupRef.Split("/").Last(), out groupGuid);

		if (!canParse)
			return false;

		if (!(_userService.isUserInGroup(user, groupGuid).Result))
		{
			return false;
		}

		serviceTypeName = form.ServiceTypeRef.Split("/").Last();


		if (_serviceTypeService.Get(Uri.UnescapeDataString(serviceTypeName)).Result == null)
			return false;

		return true;
	}
}