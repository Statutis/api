using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Statutis.API.Form;
using Statutis.API.Models;
using Statutis.Core.Interfaces.Business;
using Statutis.Entity;

namespace Statutis.API.Controllers;

[Route("api/teams/")]
[ApiController]
public class TeamController : Controller
{
	private ITeamService _teamService;
	private readonly IUserService _userService;

	public TeamController(ITeamService teamService, IUserService _userService)
	{
		_teamService = teamService;
		this._userService = _userService;
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


	[HttpPut, Route("{guid}"), Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeamModel))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update(Guid guid, [FromBody] TeamForm form)
	{
		Team? team = await _teamService.Get(guid);
		if (team == null)
			return NotFound();

		if ((HttpContext.User.Identity?.IsAuthenticated ?? false) == false)
			return Forbid();

		var user = await _userService.GetUserAsync(User);
		if (user.Roles != "ROLE_ADMIN" && !team.Users.Contains(user))
			return Forbid();

		team.Name = form.Name;
		team.Color = form.Color;

		team.Users.Clear();

		foreach (string formTeam in form.Users)
		{
			var teamId = formTeam.Replace(Url.Action("GetByEmail", "User", new { email = "<d>" })?.Replace("%3Cd%3E", "") ?? "", "");
			team.Users.Add((await _userService.GetByEmail(teamId))!);
		}
		
		if (user.Roles != "ROLE_ADMIN" && !team.Users.Contains(user))
			team.Users.Add(user);

		await _teamService.Update(team);

		return Ok(new TeamModel(team, Url));
	}

	[HttpPost, Route(""), Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeamModel))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Add([FromBody] TeamForm form)
	{

		Team team = new Team(form.Name, form.Color);
		foreach (string formTeam in form.Users)
		{
			var teamId = formTeam.Replace(Url.Action("GetByEmail", "User", new { email = "<d>" })?.Replace("%3Cd%3E", "") ?? "", "");
			team.Users.Add((await _userService.GetByEmail(teamId))!);
		}
		
		if ((HttpContext.User.Identity?.IsAuthenticated ?? false) == false)
			return Forbid();

		var user = await _userService.GetUserAsync(User);
		if (user.Roles != "ROLE_ADMIN" && !team.Users.Contains(user))
			team.Users.Add(user);
		
		await _teamService.Add(team);

		return Ok(new TeamModel(team, Url));
	}
	
	[HttpDelete, Route("{guid}"), Authorize]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Delete(Guid guid)
	{
		Team? team = await _teamService.Get(guid);
		if (team == null)
			return NotFound();

		if ((HttpContext.User.Identity?.IsAuthenticated ?? false) == false)
			return Forbid();

		var user = await _userService.GetUserAsync(User);
		if (user.Roles != "ROLE_ADMIN" && !team.Users.Contains(user))
			return Forbid();


		await _teamService.Delete(team);
			
		
		return Ok();

	}
}