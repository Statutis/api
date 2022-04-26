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

    [HttpPost("add/dns")]
    [Authorize]
    public async Task<IActionResult> Add([FromBody] DnsForm form)
    {
        if ((HttpContext.User.Identity?.IsAuthenticated ?? false) == false)
            return Forbid();

        User user = await _userService.GetUserAsync(User);

        bool canParse = Guid.TryParse(form.GroupRef.Split("/").Last(), out Guid groupId);

        if (!canParse)
            return Forbid();
        
        if (!(await _userService.isUserInGroup(user, groupId)))
        {
            return Forbid();
        }

        string serviceTypeName = form.ServiceTypeRef.Split("/").Last();

        if (await _serviceTypeService.Get(serviceTypeName) == null)
            return Forbid();

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