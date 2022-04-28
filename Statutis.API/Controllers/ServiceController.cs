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

[Route("api/services")]
[ApiController]
public class ServiceController : Controller
{
    private IHistoryEntryService _historyEntryService;
    private IServiceService _serviceService;
    private readonly IUserService _userService;
    private readonly IServiceTypeService _serviceTypeService;

    public ServiceController(IHistoryEntryService historyEntryService, IServiceService service,
        IUserService userService, IServiceTypeService serviceTypeService)
    {
        _historyEntryService = historyEntryService;
        _serviceService = service;
        _userService = userService;
        _serviceTypeService = serviceTypeService;
    }

    private bool canAddService(ServiceForm form, out Guid groupGuid, out string serviceTypeName)
    {
        groupGuid = new Guid();
        serviceTypeName = "";
        
        if ((HttpContext.User.Identity?.IsAuthenticated ?? false) == false)
            return false;

        User user = _userService.GetUserAsync(User).Result;

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
    
    [HttpPost("dns")]
    [Authorize]
    public async Task<IActionResult> AddDns([FromBody] DnsForm form)
    {
        bool status = canAddService(form, out Guid groupGuid, out string serviceTypeName);
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

    [HttpPost("http")]
    public async Task<IActionResult> AddHttpService([FromBody]HttpForm form)
    {
        form.ServiceTypeRef = Uri.UnescapeDataString(form.ServiceTypeRef);
        bool status = canAddService(form, out Guid groupGuid, out string serviceTypeName);
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
            Port = form.Port,
            Code = form.Code,
            RedirectUrl = form.RedirectUrl
        };

        Service service = await _serviceService.Insert(httpService);
        return Ok(new HttpServiceModel(httpService, HistoryState.Unknown, Url));
    }

    [HttpPost, Route("ping")]
    public async Task<IActionResult> AddPingService([FromBody] PingForm form)
    {
        bool status = canAddService(form, out Guid groupGuid, out string serviceTypeName);
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

    [HttpGet, Route("checks")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<String>))]
    public Task<IActionResult> GetCheckType()
    {
        return Task.FromResult<IActionResult>(Ok(new List<String>()
            {DnsService.CheckType, HttpService.CheckType, PingService.CheckType}));
    }

    [HttpGet, Route("state")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MainStateModel))]
    public async Task<IActionResult> GetAll()
    {
        var res = await _historyEntryService.GetMainState();
        return Ok(new MainStateModel() {State = res.Item1, LastUpdate = res.Item2});
    }

    [HttpGet, Route("{guid}")]
    // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MainStateModel))]
    public async Task<IActionResult> Get(Guid guid)
    {
        var res = await _serviceService.Get(guid);
        var user = await _userService.GetByEmail(User.Identity.Name);

        if (User.Identity is {IsAuthenticated: false} && await _userService.IsUserInTeam(user, res.Group.Teams))
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new AuthModel(null, Url));
        }

        return Ok(new ServiceModel(res, res.HistoryEntries.Last(), Url));
    }
}