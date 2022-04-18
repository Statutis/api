using Microsoft.AspNetCore.Mvc;
using Statutis.API.Models;
using Statutis.Core.Interfaces.Business;
using Statutis.Entity;

namespace Statutis.API.Controllers;

[Route("api/team/")]
[ApiController]
public class TeamController : Controller
{
	private ITeamService _teamService;

	public TeamController(ITeamService teamService)
	{
		_teamService = teamService;
	}

	[HttpGet, Route("")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeamModel))]
	public async Task<IActionResult> GetAll()
	{
		return Ok((await _teamService.GetAll()).Select(x => new TeamModel(x, this.Url)));
	}

	[HttpGet, Route("{guid}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeamModel))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetGuid(Guid guid)
	{
		Team? type = await _teamService.Get(guid);
		if (type == null)
			return NotFound();
		return Ok(new TeamModel(type, this.Url));
	}
}