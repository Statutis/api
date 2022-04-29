using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Statutis.API.Form;
using Statutis.API.Models;
using Statutis.Core.Interfaces.Business;
using Statutis.Core.Interfaces.Business.History;
using Statutis.Core.Interfaces.Business.Service;
using Statutis.Entity.Service;

namespace Statutis.API.Controllers;

/// <summary>
/// Controleur sur les goupes
/// </summary>
[Tags("Groupe")]
[Route("api/groups")]
[ApiController]
public class GroupController : Controller
{
	private IHistoryEntryService _historyEntryService;
	private IGroupService _groupService;
	private readonly IUserService _userService;
	private readonly ITeamService _teamService;

	/// <summary>
	/// Constructeur
	/// </summary>
	/// <param name="historyEntryService"></param>
	/// <param name="groupService"></param>
	/// <param name="userService"></param>
	/// <param name="teamService"></param>
	public GroupController(IHistoryEntryService historyEntryService, IGroupService groupService, IUserService userService, ITeamService teamService)
	{
		_historyEntryService = historyEntryService;
		_groupService = groupService;
		_userService = userService;
		this._teamService = teamService;
	}

	/// <summary>
	/// Récupration de tous les groupes
	/// </summary>
	/// <returns>Groupes public si pas authentifié, sinon tous les groupes existants</returns>
	[HttpGet, Route("")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GroupModel>))]
	public async Task<IActionResult> GetAll()
	{
		var res = (await _groupService.GetPublicGroup());

		if (HttpContext.User.Identity?.IsAuthenticated ?? false)
		{
			var user = await _userService.GetUserAsync(User);
			if (user != null)
				res.AddRange(await _groupService.GetFromUser(user));
		}

		return Ok(
			res.Distinct()
				.Select(async x => new GroupModel(x, await _historyEntryService.GetAllLast(x.Services), Url))
				.Select(x => x.Result).ToList()
		);
	}

	/// <summary>
	/// Récupération d'un groupe par son identifiant
	/// </summary>
	/// <param name="guid">Identifiant du groupe cible</param>
	/// <returns>Un groupe</returns>
	/// <response code="403">Si vous ne diposez pas des accès suivant pour consulter ces informations.</response>
	[HttpGet, Route("{guid}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GroupModel))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
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
			if (user == null)
				return Forbid();

			var teamUserId = user.Teams.Select(x => x.TeamId);
			if (!group.Teams.Any(x => teamUserId.Contains(x.TeamId)))
				return Forbid();
		}

		var histories = await _historyEntryService.GetAllLast(group.Services);

		return Ok(new GroupModel(group, histories, Url));
	}

	/// <summary>
	///	Mise a jour d'un groupe
	/// </summary>
	/// <param name="guid">Identifiant du groupe que l'on souhaite mettre à jour</param>
	/// <param name="form">Nouvelles informations sur le groupe</param>
	/// <returns></returns>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	/// <response code="403">Si vous ne diposez pas des accès suivant pour consulter ces informations.</response>
	[HttpPut, Route("{guid}"), Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GroupModel))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> Update(Guid guid, [FromBody] GroupForm form)
	{
		Group? group = await _groupService.Get(guid);
		if (group == null)
			return NotFound();


		if ((HttpContext.User.Identity?.IsAuthenticated ?? false) == false)
			return Forbid();


		var user = await _userService.GetUserAsync(User);
		if (user == null)
			return Forbid();

		var teamUserId = user.Teams.Select(x => x.TeamId);
		if (!group.Teams.Any(x => teamUserId.Contains(x.TeamId)))
			return Forbid();


		group.Name = form.Name;
		group.Description = form.Description;
		group.IsPublic = form.IsPublic;


		group.Teams.Clear();

		foreach (string formTeam in form.Teams)
		{
			var teamId = formTeam.Replace(Url.Action("GetGuid", "Team", new { guid = "<d>" })?.Replace("%3Cd%3E", "") ?? "", "");
			group.Teams.Add((await _teamService.Get(Guid.Parse(teamId)))!);
		}

		await _groupService.Update(group);

		var histories = await _historyEntryService.GetAllLast(group.Services);
		return Ok(new GroupModel(group, histories, Url));

	}

	/// <summary>
	///	Ajout d'un groupe
	/// </summary>
	/// <param name="form">Informations sur le nouveau groupe</param>
	/// <returns></returns>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	[HttpPost, Route(""), Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GroupModel))]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> Add([FromBody] GroupForm form)
	{
		Group group = new Group(form.Name, form.Description);

		group.IsPublic = form.IsPublic;

		group.Teams.Clear();

		foreach (string formTeam in form.Teams)
		{
			var teamId = formTeam.Replace(Url.Action("GetGuid", "Team", new { guid = "<d>" })?.Replace("%3Cd%3E", "") ?? "", "");
			group.Teams.Add((await _teamService.Get(Guid.Parse(teamId)))!);
		}

		await _groupService.Insert(group);

		var histories = await _historyEntryService.GetAllLast(group.Services);
		return Ok(new GroupModel(group, histories, Url));

	}

	/// <summary>
	///	Suppression d'un groupe
	/// </summary>
	/// <param name="guid">Identifiant du groupe que l'on souhaite supprimer</param>
	/// <returns></returns>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	/// <response code="403">Si vous ne diposez pas des accès suivant pour consulter ces informations.</response>
	[HttpDelete, Route("{guid}"), Authorize]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> Delete(Guid guid)
	{
		Group? group = await _groupService.Get(guid);
		if (group == null)
			return NotFound();


		if ((HttpContext.User.Identity?.IsAuthenticated ?? false) == false)
			return Forbid();


		var user = await _userService.GetUserAsync(User);
		if (user == null)
			return Forbid();

		var teamUserId = user.Teams.Select(x => x.TeamId);
		if (!group.Teams.Any(x => teamUserId.Contains(x.TeamId)))
			return Forbid();


		await _groupService.Delete(group);


		return Ok();

	}

	/// <summary>
	/// Récupération d'un avatar d'un groupe
	/// </summary>
	/// <param name="guid">Identifiant du groupe cible</param>
	/// <remarks>Lorsque l'on est pas authentifié, il n'est possible de récupérer que les avatars des groupes publics</remarks>
	/// <returns></returns>
	/// <response code="404">Si le groupe visé n'existe pas ou qu'il ne dispose pas d'avatar.</response>
	/// <response code="403">Vous ne disposez pas des droits suffisants.</response>
	[HttpGet]
	[AllowAnonymous]
	[Route("avatar/{guid}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> GetAvatar(Guid guid)
	{
		var targetGroup = await _groupService.Get(guid);
		if (targetGroup == null || targetGroup.Avatar == null || targetGroup.AvatarContentType == null)
			return NotFound();

		if ((HttpContext.User.Identity?.IsAuthenticated ?? false) == false && !targetGroup.IsPublic)
			return Forbid();

		return File(targetGroup.Avatar, targetGroup.AvatarContentType);
	}

	/// <summary>
	/// Mettre à jour l'avatar d'un groupe
	/// </summary>
	/// <param name="guid">Identifiant du groupe cible</param>
	/// <param name="form">Informations sur le nouvel avatar (null si l'on souhaite supprimer celui courant)</param>
	/// <returns></returns>
	/// 
	/// <remarks>Il n'est possible de modifier l'avatar que si l'utilisateur à un accès</remarks>
	/// <response code="404">Si le groupe visé n'existe pas.</response>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	/// <response code="403">Vous ne disposez pas des droits suffisants.</response>
	[HttpPut, Authorize]
	[Authorize]
	[Route("avatar/{guid}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> UploadAvatar(Guid guid, IFormFile? form = null)
	{

		var user = await _userService.GetUserAsync(User);
		if (user == null)
			return StatusCode(401, new AuthModel(null, Url));

		var targetGroup = await _groupService.Get(guid);
		var userGroups = await _groupService.GetFromUser(user);

		if (targetGroup == null || (!user.IsAdmin() && userGroups.Contains(targetGroup)))
			return Forbid();


		if (form == null)
		{
			targetGroup.Avatar = null;
			targetGroup.AvatarContentType = null;
		}
		else
		{
			using (var memoryStream = new MemoryStream())
			{
				await form.CopyToAsync(memoryStream);
				targetGroup.Avatar = memoryStream.ToArray();
			}
			targetGroup.AvatarContentType = form.ContentType;
		}


		await _groupService.Update(targetGroup);

		return Ok();
	}


}