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
		if (user == null || user.Roles != "ROLE_ADMIN" && !team.Users.Contains(user))
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
		if (user != null && user.Roles != "ROLE_ADMIN" && !team.Users.Contains(user))
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
		if (user != null && user.Roles != "ROLE_ADMIN" && !team.Users.Contains(user))
			return Forbid();


		await _teamService.Delete(team);


		return Ok();

	}

	[HttpGet]
	[AllowAnonymous]
	[Route("avatar/{guid}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetAvatar(Guid guid)
	{
		var targetTeam = await _teamService.Get(guid);
		if (targetTeam == null || targetTeam.Avatar == null || targetTeam.AvatarContentType == null)
			return NotFound();

		//Todo : Vérifier si il s'agit d'une équipe publique

		return File(targetTeam.Avatar, targetTeam.AvatarContentType);
	}

	[HttpPut, Authorize]
	[Authorize]
	[Route("avatar/{guid}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> UploadAvatar(Guid guid, IFormFile? form = null)
	{

		var user = await _userService.GetUserAsync(User);
		if (user == null)
			return StatusCode(401, new AuthModel(null, Url));

		var targetTeam = await _teamService.Get(guid);
		var userTeam = await _teamService.GetTeamsOfUser(user);
		if (targetTeam == null || (user.Roles != "ROLE_ADMIN" && userTeam.Contains(targetTeam)))
			return NotFound();


		if (form == null)
		{
			targetTeam.Avatar = null;
			targetTeam.AvatarContentType = null;
		}
		else
		{
			using (var memoryStream = new MemoryStream())
			{
				await form.CopyToAsync(memoryStream);
				targetTeam.Avatar = memoryStream.ToArray();
			}
			targetTeam.AvatarContentType = form.ContentType;
		}


		await _teamService.Update(targetTeam);

		return Ok();
	}
}