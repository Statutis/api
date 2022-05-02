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
    private readonly IGroupService _groupService;

    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="historyEntryService"></param>
    /// <param name="service"></param>
    /// <param name="userService"></param>
    /// <param name="serviceTypeService"></param>
    /// <param name="groupService"></param>
    public ServiceController(IHistoryEntryService historyEntryService,
        IServiceService service,
        IUserService userService,
        IServiceTypeService serviceTypeService,
        IGroupService groupService)
    {
        _historyEntryService = historyEntryService;
        _serviceService = service;
        _userService = userService;
        _serviceTypeService = serviceTypeService;
        _groupService = groupService;
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
        form.ServiceTypeRef = Uri.UnescapeDataString(form.ServiceTypeRef);
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
            {DnsService.CheckType, HttpService.CheckType, PingService.CheckType, AtlassianStatusPageService.CheckType}));
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
        return Ok(new MainStateModel() {State = res.Item1, LastUpdate = res.Item2});
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
        var group = await _groupService.Get(res.GroupId);
        if (group == null)
            return NotFound();


        var user = await _userService.GetUserAsync(User);
        if (user == null || !_userService.IsUserInTeam(user, res.Group.Teams))
            return Forbid();

        var test = (res.HistoryEntries.Count > 0) ? res.HistoryEntries.Last() : new HistoryEntry();


        return Ok(new ServiceModel(res, test, Url));
    }

    /// <summary>
    /// Permet de récupérer un service de type DNS
    /// </summary>
    /// <param name="guid"></param>
    /// <response code="401">Si vous n'êtes pas authentifié.</response>
    /// <response code="404">Si le service n'est pas trouvé.</response>
    /// <response code="403">Si l'utilisateur courant n'a pas les droits sur le service cible.</response>
    /// <response code="200">Retourne le service demandé</response>
    /// <returns>un objet de type service</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DnsService))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet("dns/{guid}")]
    public async Task<IActionResult> GetDns(Guid guid)
    {
        User? user = await _userService.GetUserAsync(User);
        if (user == null)
            return Unauthorized();
        
        bool check = canUserViewService(user, guid, out Service? service);
        if (!check)
            return Forbid();

        var dnsService = await _serviceService.GetByClass<DnsService>(guid);
        if (dnsService == null)
            return new StatusCodeResult(StatusCodes.Status404NotFound);

        var historyState = (dnsService.HistoryEntries.Count > 0)
            ? dnsService.HistoryEntries.Last().State
            : HistoryState.Unknown;

        return Ok(new DnsServiceModel(dnsService, historyState, Url));
    }


    /// <summary>
    /// Permet de récupérer un service de type http
    /// </summary>
    /// <param name="guid"></param>
    /// <response code="401">Si vous n'êtes pas authentifié.</response>
    /// <response code="404">Si le service n'est pas trouvé.</response>
    /// <response code="403">Si l'utilisateur courant n'a pas les droits sur le service cible.</response>
    /// <response code="200">Retourne le service demandé</response>
    /// <returns>un objet de type service</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DnsService))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet("http/{guid}")]
    public async Task<IActionResult> GetHttp(Guid guid)
    {
        User? user = await _userService.GetUserAsync(User);
        if (user == null)
            return Unauthorized();
        bool check = canUserViewService(user, guid, out Service? service);
        if (!check && !user.IsAdmin())
            return Forbid();

        var httpService = await _serviceService.GetByClass<HttpService>(guid);
        if (httpService == null)
            return Forbid();

        var historyState = (httpService.HistoryEntries.Count > 0)
            ? httpService.HistoryEntries.Last().State
            : HistoryState.Unknown;

        return Ok(new HttpServiceModel(httpService, historyState, Url));
    }

    /// <summary>
    /// Permet de récupérer un service de type ping
    /// </summary>
    /// <param name="guid"></param>
    /// <response code="401">Si vous n'êtes pas authentifié.</response>
    /// <response code="404">Si le service n'est pas trouvé.</response>
    /// <response code="403">Si l'utilisateur courant n'a pas les droits sur le service cible.</response>
    /// <response code="200">Retourne le service demandé</response>
    /// <returns>un objet de type service</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DnsService))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet("ping/{guid}")]
    public async Task<IActionResult> GetPing(Guid guid)
    {
        User? user = await _userService.GetUserAsync(User);
        if (user == null)
            return Unauthorized();
        bool check = canUserViewService(user, guid, out Service? service);
        if (!check)
            return Forbid();

        var pingService = await _serviceService.GetByClass<PingService>(guid);
        if (pingService == null)
            return Forbid();

        var historyState = (pingService.HistoryEntries.Count > 0)
            ? pingService.HistoryEntries.Last().State
            : HistoryState.Unknown;

        return Ok(new PingServiceModel(pingService, historyState, Url));
    }

    /// <summary>
    /// Permet de récupérer un service de type DNS
    /// </summary>
    /// <param name="guid"></param>
    /// <response code="401">Si vous n'êtes pas authentifié.</response>
    /// <response code="404">Si le service n'est pas trouvé.</response>
    /// <response code="403">Si l'utilisateur courant n'a pas les droits sur le service cible.</response>
    /// <response code="200">Retourne le service demandé</response>
    /// <returns>un objet de type service</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DnsService))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet("atlassian_status_page/{guid}")]
    public async Task<IActionResult> GetAtlassianStatusPage(Guid guid)
    {
        User? user = await _userService.GetUserAsync(User);
        if (user == null)
            return Unauthorized();
        bool check = canUserViewService(user, guid, out Service? service);
        if (!check)
            return Forbid();

        var statusPageService = await _serviceService.GetByClass<AtlassianStatusPageService>(guid);
        if (statusPageService == null)
            return Forbid();

        var historyState = (statusPageService.HistoryEntries.Count > 0)
            ? statusPageService.HistoryEntries.Last().State
            : HistoryState.Unknown;

        return Ok(new AtlassianStatusPageModel(statusPageService, historyState, Url));
    }

    /// <summary>
    /// Permet de mettre à jour un service type DNS
    /// </summary>
    /// <param name="form"></param>
    /// <param name="guid"></param>
    /// <returns>DNS</returns>
    [HttpPut("dns/{guid}"), Authorize]
    public async Task<IActionResult> UpdateDnsService(Guid guid, [FromBody] DnsPatchForm form)
    {
        bool status = CanUpdateService(form, out Guid groupGuid, out string serviceTypeName);
        if (!status)
            return Forbid();

        DnsService? service = await _serviceService.GetByClass<DnsService>(guid);
        if (service == null)
            return Forbid();

        string serviceTypeRefStr = form.ServiceTypeRef.Split("/").Last();
        serviceTypeRefStr = Uri.UnescapeDataString(serviceTypeRefStr);
        ServiceType? serviceType = await _serviceTypeService.Get(serviceTypeRefStr);
        if (serviceType == null)
            return Forbid();


        Group? group = await _groupService.Get(groupGuid);
        if (group == null)
            return Forbid();
        
        service.Description = form.Description;
        service.Host = form.Host;
        service.Name = form.Name;
        service.ServiceTypeName = serviceType.Name;
        service.GroupId = groupGuid;
        service.Result = form.Result;
        service.Type = form.Type;

        var historyState = (service.HistoryEntries.Count > 0)
            ? service.HistoryEntries.Last().State
            : HistoryState.Unknown;
        
        var updateObj = await _serviceService.Update<DnsService>(service);
        return Ok(new DnsServiceModel(updateObj, historyState, Url));
    }
    
    /// <summary>
    /// Permet de mettre à jour un service type Http
    /// </summary>
    /// <param name="guid"></param>
    /// <param name="form"></param>
    /// <returns>HTTP</returns>
    [HttpPut("http/{guid}"), Authorize]
    public async Task<IActionResult> UpdateHttpService(Guid guid, [FromBody] HttpPatchForm form)
    {
        bool status = CanUpdateService(form, out Guid groupGuid, out string serviceTypeName);
        if (!status)
            return Forbid();
        
        HttpService? service = await _serviceService.GetByClass<HttpService>(guid);
        if (service == null)
            return Forbid();

        string serviceTypeRefStr = form.ServiceTypeRef.Split("/").Last();
        serviceTypeRefStr = Uri.UnescapeDataString(serviceTypeRefStr);
        ServiceType? serviceType = await _serviceTypeService.Get(serviceTypeRefStr);
        if (serviceType == null)
            return Forbid();

        Group? group = await _groupService.Get(groupGuid);
        if (group == null)
            return Forbid();
        
        service.Description = form.Description;
        service.Host = form.Host;
        service.Name = form.Name;
        service.ServiceTypeName = serviceType.Name;
        service.GroupId = groupGuid;
        service.Code = form.Code;
        service.RedirectUrl = form.RedirectUrl;

        var historyState = (service.HistoryEntries.Count > 0)
            ? service.HistoryEntries.Last().State
            : HistoryState.Unknown;
        
        var updateObj = await _serviceService.Update<HttpService>(service);
        return Ok(new HttpServiceModel(updateObj, historyState, Url));
    }
    
    /// <summary>
    /// Permet de mettre à jour un service type Ping
    /// </summary>
    /// <param name="form"></param>
    /// <param name="guid"></param>
    /// <returns>ping</returns>
    [HttpPut("ping/{guid}"), Authorize]
    public async Task<IActionResult> UpdatePingService(Guid guid, [FromBody] PingPatchForm form)
    {
        bool status = CanUpdateService(form, out Guid groupGuid, out string serviceTypeName);
        if (!status)
            return Forbid();
        
        
        PingService? service = await _serviceService.GetByClass<PingService>(guid);
        if (service == null)
            return Forbid();

        string serviceTypeRefStr = form.ServiceTypeRef.Split("/").Last();
        serviceTypeRefStr = Uri.UnescapeDataString(serviceTypeRefStr);
        ServiceType? serviceType = await _serviceTypeService.Get(serviceTypeRefStr);
        if (serviceType == null)
            return Forbid();

        Group? group = await _groupService.Get(groupGuid);
        if (group == null)
            return Forbid();
        
        service.Description = form.Description;
        service.Host = form.Host;
        service.Name = form.Name;
        service.ServiceTypeName = serviceType.Name;
        service.GroupId = groupGuid;

        var historyState = (service.HistoryEntries.Count > 0)
            ? service.HistoryEntries.Last().State
            : HistoryState.Unknown;
        
        var updateObj = await _serviceService.Update<PingService>(service);
        return Ok(new PingServiceModel(updateObj, historyState, Url));
    }
    
    /// <summary>
    /// Permet de mettre à jour un service type Atlassian Status Page
    /// </summary>
    /// <param name="form"></param>
    /// <param name="guid"></param>
    /// <returns>DNS</returns>
    [HttpPut("atlassian_status_page/{guid}"), Authorize]
    public async Task<IActionResult> UpdateAtlassianService(Guid guid, [FromBody] AtlassianStatusPagePatchForm form)
    {
        bool status = CanUpdateService(form, out Guid groupGuid, out string serviceTypeName);
        if (!status)
            return Forbid();

        AtlassianStatusPageService? service = await _serviceService.GetByClass<AtlassianStatusPageService>(guid);
        if (service == null)
            return Forbid();

        string serviceTypeRefStr = form.ServiceTypeRef.Split("/").Last();
        serviceTypeRefStr = Uri.UnescapeDataString(serviceTypeRefStr);
        ServiceType? serviceType = await _serviceTypeService.Get(serviceTypeRefStr);
        if (serviceType == null)
            return Forbid();

        Group? group = await _groupService.Get(groupGuid);
        if (group == null)
            return Forbid();
        
        service.Description = form.Description;
        service.Host = form.Host;
        service.Name = form.Name;
        service.ServiceTypeName = serviceType.Name;
        service.GroupId = groupGuid;

        var historyState = (service.HistoryEntries.Count > 0)
            ? service.HistoryEntries.Last().State
            : HistoryState.Unknown;
        
        var updateObj = await _serviceService.Update<AtlassianStatusPageService>(service);
        return Ok(new AtlassianStatusPageModel(updateObj, historyState, Url));
    }


    /// <summary>
    /// Suppression d'un service
    /// </summary>
    /// <param name="guid"></param>
    /// <response code="401">Si vous n'êtes pas authentifié.</response>
    /// <response code="404">Si le service n'est pas trouvé.</response>
    /// <response code="403">Si l'utilisateur courant n'a pas les droits sur le groupe cible.</response>
    /// <returns>Réponse vide</returns>
    [HttpDelete, Route("{guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> deleteService(Guid guid)
    {
        if ((HttpContext.User.Identity?.IsAuthenticated ?? false) == false)
            return new StatusCodeResult(StatusCodes.Status401Unauthorized);

        User? user = await _userService.GetUserAsync(User);
        if (user == null)
            return new StatusCodeResult(StatusCodes.Status401Unauthorized);

        bool flag = await _userService.IsUserInGroup(user, guid);
        if (!flag && !user.IsAdmin())
            return Forbid();

        Service? service = await _serviceService.Get(guid);
        if (service == null)
            return Forbid();

        await _serviceService.Delete(service);

        return Ok();
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

        if (!(_userService.IsUserInGroup(user, groupGuid).Result))
        {
            return false;
        }

        serviceTypeName = form.ServiceTypeRef.Split("/").Last();


        if (_serviceTypeService.Get(Uri.UnescapeDataString(serviceTypeName)).Result == null)
            return false;

        return true;
    }

    /// <summary>
    /// Vérification s'il est possible de modifier un service
    /// </summary>
    /// <param name="form">Formulaire du service</param>
    /// <param name="groupGuid">Groupe cible</param>
    /// <param name="serviceTypeName">Type de service cible</param>
    /// <returns></returns>
    private bool CanUpdateService(ServicePatchForm form, out Guid groupGuid, out string serviceTypeName)
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

        if (!(_userService.IsUserInGroup(user, groupGuid).Result))
        {
            return false;
        }

        serviceTypeName = form.ServiceTypeRef.Split("/").Last();


        if (_serviceTypeService.Get(Uri.UnescapeDataString(serviceTypeName)).Result == null)
            return false;

        return true;
    }

    /// <summary>
    /// Permet de savoir si un utilisateur est authorisé à voir un service
    /// </summary>
    /// <param name="user"></param>
    /// <param name="guid"></param>
    /// <param name="service"></param>
    /// <returns></returns>
    private bool canUserViewService(User user, Guid guid, out Service? service)
    {
        service = _serviceService.Get(guid).Result;
        if (service == null)
            return false;

        Group? group = _groupService.Get(service.GroupId).Result;
        if (group == null)
        {
            return false;
        }

        foreach (Team team in user.Teams)
        {
            bool check = team.Groups.Contains(group);
            if (check)
                return true;
        }

        return false;
    }
}