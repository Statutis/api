using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Statutis.API.Models;
using Statutis.Core.Interfaces.Business.History;
using Statutis.Core.Interfaces.Business.Service;
using Statutis.Entity.History;

namespace Statutis.API.Controllers;

[Route("api/history")]
[ApiController]
public class HistoryController : Controller
{

    private readonly IHistoryEntryService _historyEntryService;
    private readonly IServiceService _serviceService;

    public HistoryController(IHistoryEntryService historyEntryService, IServiceService serviceService)
    {
        _historyEntryService = historyEntryService;
        _serviceService = serviceService;
    }
    
    [HttpGet("service/{guid}")]
    public async Task<IActionResult> Get([Required]Guid guid)
    {
        var service = await _serviceService.Get(guid);
        if (service == null)
        {
            return Forbid();
        }
        var history = await _historyEntryService.Get(service, 30);
        List<HistoryEntryModel> list = new List<HistoryEntryModel>();
        foreach (HistoryEntry historyEntry in history)
        {
            list.Add(new HistoryEntryModel(historyEntry, Url));
        }

        return Ok(list);
    }
}