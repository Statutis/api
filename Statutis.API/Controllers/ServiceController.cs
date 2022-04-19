using Microsoft.AspNetCore.Mvc;
using Statutis.API.Models;
using Statutis.Core.Interfaces.Business.History;
using Statutis.Entity.History;

namespace Statutis.API.Controllers;

[Route("api/services")]
[ApiController]
public class ServiceController : Controller
{
	private IHistoryEntryService _historyEntryService;

	public ServiceController(IHistoryEntryService historyEntryService)
	{
		_historyEntryService = historyEntryService;
	}

	[HttpGet, Route("state")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MainStateModel))]
	public async Task<IActionResult> GetAll()
	{
		var res = await _historyEntryService.GetMainState();
		return Ok(new MainStateModel() { State = res.Item1, LastUpdate = res.Item2 });
	}

	[HttpGet, Route("{guid}")]
	// [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MainStateModel))]
	public async Task<IActionResult> Get(Guid guid)
	{
		
		return Ok();
	}
}