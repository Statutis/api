using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Statutis.API.Models;
using Statutis.Core.Interfaces.Business;
using Statutis.Core.Interfaces.Business.History;
using Statutis.Core.Interfaces.Business.Service;
using Statutis.Entity;
using Statutis.Entity.Service;

namespace Statutis.API.Controllers;

[Route("api/groups")]
[ApiController]
public class GroupController : Controller
{
	private IHistoryEntryService _historyEntryService;
	private IGroupService _groupService;
	private readonly IUserService _userService;

	public GroupController(IHistoryEntryService historyEntryService, IGroupService groupService, IUserService userService)
	{
		_historyEntryService = historyEntryService;
		_groupService = groupService;
		_userService = userService;
	}

	[HttpGet, Route("")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GroupModel>))]
	public async Task<IActionResult> GetAll()
	{
		var res = (await _groupService.GetPublicGroup());

		if (HttpContext.User.Identity?.IsAuthenticated ?? false)
		{
			var user = await _userService.GetUserAsync(User);
			res.AddRange(await _groupService.GetFromUser(user));
		}

		return Ok(
			res.Distinct()
				.Select(async x => new GroupModel(x, await _historyEntryService.GetAllLast(x.Services), Url))
				.Select(x => x.Result).ToList()
		);
	}

	[HttpGet, Route("{guid}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GroupModel))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Get(Guid guid)
	{
		Group? group = await _groupService.Get(guid);
		if (group == null)
			return NotFound();

		if (!group.IsPublic)
		{
			if ((HttpContext.User.Identity?.IsAuthenticated ?? false) == false)
				return Forbid();
			
			
			var user = await _userService.GetUserAsync(User);
			var teamUserId = user.Teams.Select(x => x.TeamId);
			if (!group.Teams.Any(x => teamUserId.Contains(x.TeamId)))
				return Forbid();
		}

		var histories = await _historyEntryService.GetAllLast(group.Services);

		return Ok(new GroupModel(group, histories, Url));
	}

}