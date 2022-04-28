using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Statutis.API.Form;
using Statutis.API.Models;
using Statutis.Core.Interfaces.Business;
using Statutis.Entity;

namespace Statutis.API.Controllers;


/// <summary>
/// Controleur sur les équipes
/// </summary>
[Tags("Equipe")]
[Route("api/teams/")]
[ApiController]
public class TeamController : Controller
{
	private ITeamService _teamService;
	private readonly IUserService _userService;

	/// <summary>
	/// Constructeur
	/// </summary>
	/// <param name="teamService"></param>
	/// <param name="userService"></param>
	public TeamController(ITeamService teamService, IUserService userService)
	{
		_teamService = teamService;
		this._userService = userService;
	}

	/// <summary>
	/// Récupération de toutes les équipes
	/// </summary>
	/// <remarks>Si pas authentifié, récupération seulement des équipes publiques, sinon récupération de toutes les équipes existantes.</remarks>
	/// <returns>Liste d'équipes</returns>
	[HttpGet, Route("")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeamModel))]
	public async Task<IActionResult> GetAll()
	{
		//TODO limiter seulement au équipes qui on un groupe public
		return Ok((await _teamService.GetAll()).Select(x => new TeamModel(x, this.Url)));
	}

	/// <summary>
	/// Récupération d'une équipe
	/// </summary>
	/// <param name="guid">Identifiant de l'équipe cible</param>
	/// <remarks>Si pas authentifié, récupération possible seulement des équipes publiques, sinon pas de limitations.</remarks>
	/// <returns>Une équipe</returns>
	/// <response code="404">Si l'équipe visée n'existe pas.</response>
	/// <response code="403">Vous ne disposez pas des droits suffisants.</response>
	[HttpGet, Route("{guid}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeamModel))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetGuid(Guid guid)
	{
		// TODO droits
		Team? type = await _teamService.Get(guid);
		if (type == null)
			return NotFound();
		return Ok(new TeamModel(type, this.Url));
	}

	/// <summary>
	/// Mise à jour d'une équipe
	/// </summary>
	/// <param name="guid">Identifiant de l'équipe cible</param>
	/// <param name="form">Nouvelles informations sur l'équipe</param>
	/// <remarks>
	/// Pour pouvoir modifier une équipe il faut soit être administrateur, soit être membre de cette équipe.
	/// </remarks>
	/// <returns>Une équipe</returns>
	/// <response code="404">Si l'équipe visée n'existe pas.</response>
	/// <response code="403">Vous ne disposez pas des droits suffisants.</response>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	[HttpPut, Route("{guid}"), Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeamModel))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

	/// <summary>
	/// Ajout d'une équipe
	/// </summary>
	/// <param name="form">Nouvelles informations de la nouvelle équipe</param>
	/// <returns>Une équipe</returns>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
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
			return Unauthorized();

		var user = await _userService.GetUserAsync(User);
		if (user != null && user.Roles != "ROLE_ADMIN" && !team.Users.Contains(user))
			team.Users.Add(user);

		await _teamService.Add(team);

		return Ok(new TeamModel(team, Url));
	}

	/// <summary>
	/// Suppression d'une équipe
	/// </summary>
	/// <param name="guid">Identifiant de l'équipe cible</param>
	/// <remarks>
	/// Pour pouvoir supprimer une équipe il faut soit être administrateur, soit être membre de cette équipe.
	/// </remarks>
	/// <response code="404">Si l'équipe visée n'existe pas.</response>
	/// <response code="403">Vous ne disposez pas des droits suffisants.</response>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	[HttpDelete, Route("{guid}"), Authorize]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Delete(Guid guid)
	{
		Team? team = await _teamService.Get(guid);
		if (team == null)
			return NotFound();

		if ((HttpContext.User.Identity?.IsAuthenticated ?? false) == false)
			return Unauthorized();

		var user = await _userService.GetUserAsync(User);
		if (user != null && user.Roles != "ROLE_ADMIN" && !team.Users.Contains(user))
			return Forbid();


		await _teamService.Delete(team);


		return Ok();

	}
	
	/// <summary>
	/// Récupération d'un avatar d'une équipe
	/// </summary>
	/// <param name="guid">Identifiant d'une équipe cible</param>
	/// <returns>Avatar de l'équipe</returns>
	/// <remarks>
	/// Pour pouvoir modifier l'avatar d'une équipe il faut soit être administrateur, soit être membre de cette équipe.
	/// </remarks>
	/// <response code="404">Si l'équipe visée n'existe pas ou qu'elle ne dispose pas d'avatar.</response>
	/// <response code="403">Vous ne disposez pas des droits suffisants.</response>
	[HttpGet]
	[AllowAnonymous]
	[Route("avatar/{guid}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> GetAvatar(Guid guid)
	{
		var targetTeam = await _teamService.Get(guid);
		if (targetTeam == null || targetTeam.Avatar == null || targetTeam.AvatarContentType == null)
			return NotFound();

		//Todo : Vérifier si il s'agit d'une équipe publique

		return File(targetTeam.Avatar, targetTeam.AvatarContentType);
	}

	/// <summary>
	/// Mettre à jour l'avatar d'une équipe
	/// </summary>
	/// <param name="guid">Identifiant de l'équipe cible</param>
	/// <param name="form">Informations sur le nouvel avatar (null si l'on souhaite supprimer celui courant)</param>
	/// <returns></returns>
	/// <response code="404">Si l'équipe visée n'existe pas.</response>
	/// <response code="403">Vous ne disposez pas des droits suffisants.</response>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	[HttpPut, Authorize]
	[Authorize]
	[Route("avatar/{guid}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> UploadAvatar(Guid guid, IFormFile? form = null)
	{

		var user = await _userService.GetUserAsync(User);
		if (user == null)
			return StatusCode(401, new AuthModel(null, Url));

		var targetTeam = await _teamService.Get(guid);
		var userTeam = await _teamService.GetTeamsOfUser(user);
		if (targetTeam == null || (user.Roles != "ROLE_ADMIN" && userTeam.Contains(targetTeam)))
			return Forbid();


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