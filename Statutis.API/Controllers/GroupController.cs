using Microsoft.AspNetCore.Mvc;
using Statutis.API.Models;
using Statutis.Core.Interfaces.Business.History;
using Statutis.Core.Interfaces.Business.Service;

namespace Statutis.API.Controllers;

[Route("api/groups")]
[ApiController]
public class GroupController : Controller
{
	private IHistoryEntryService _historyEntryService;
	private IGroupService _groupService;

	public GroupController(IHistoryEntryService historyEntryService, IGroupService groupService)
	{
		_historyEntryService = historyEntryService;
		_groupService = groupService;
	}

	[HttpGet, Route("public")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GroupModel>))]
	public async Task<IActionResult> GetAll()
	{
		var res = (await _groupService.GetPublicGroup())
			.Select(async x => new GroupModel(x, await _historyEntryService.GetAllLast(x.Services), Url))
			.Select(x => x.Result).ToList();

		return Ok(res);
	}

	[HttpGet, Route("{guid}")]
	// [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MainStateModel))]
	public async Task<IActionResult> Get(Guid guid)
	{
		return Ok();
	}

}