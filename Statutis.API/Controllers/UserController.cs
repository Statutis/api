using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Statutis.API.Form;
using Statutis.API.Models;
using Statutis.Core.Interfaces.Business;
using Statutis.Entity;

namespace Statutis.API.Controllers;

[Tags("Utilisateurs")]
[Route("api/users")]
[ApiController]
[Authorize]
public class UserController : Controller
{
	private readonly IUserService _userService;
	private readonly IPasswordHash _passwordHash;

	public UserController(IUserService userService, IPasswordHash passwordHash)
	{
		_userService = userService;
		_passwordHash = passwordHash;
	}

	/// <summary>
	/// Récupération de l'utilisateur courant
	/// </summary>
	/// <returns>Un utilisateur</returns>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	[HttpGet("me")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
	[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(AuthModel))]
	public async Task<IActionResult> Get()
	{
		if (User.Identity == null)
			return StatusCode(StatusCodes.Status401Unauthorized, new AuthModel(null, Url));

		User? user = await _userService.GetUserAsync(User);
		if (user == null)
			return StatusCode(StatusCodes.Status401Unauthorized, new AuthModel(null, Url));

		return Ok(new UserModel(user, Url));
	}

	/// <summary>
	/// Récupération d'un utilisateur
	/// </summary>
	/// <param name="email">Adresse mail de l'utilisaur cible</param>
	/// <returns>Un utilisateur</returns>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	/// <response code="404">Si l'utilisateur cible n'existe pas.</response>
	[HttpGet("email/{email}")]
	[Authorize()]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetByEmail([Required] string email)
	{
		if (email == "")
			return StatusCode(StatusCodes.Status400BadRequest, "Query parameter email is required !");
		User? user = await _userService.GetByEmail(email);

		if (user == null)
			return NotFound();

		return Ok(new UserModel(user, Url));
	}

	/// <summary>
	/// Récupération de touts les utilisateurs
	/// </summary>
	/// <returns>Liste des utilisateurs</returns>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	[HttpGet("")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserModel>))]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> GetAll()
	{

		List<User> users = await _userService.GetAll();

		return Ok(users.Select(x => new UserModel(x, Url)));
	}

	/// <summary>
	/// Modification d'un utilisateur
	/// </summary>
	/// <param name="email">Adresse mail de l'utilisaur cible</param>
	/// <returns>Un utilisateur</returns>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	/// <response code="403">Si vous n'êtes pas l'utilisateur cible ou un administrateur.</response>
	/// <response code="404">Si l'utilisateur cible n'existe pas.</response>
	[HttpPut, Route("{email}")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserPutModel))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> Update([FromBody] UserPutModel form, String email)
	{
		if (User.Identity == null || User.Identity.Name == null)
			return StatusCode(401, new AuthModel(null, Url));

		var user = await _userService.GetUserAsync(User);
		if (user == null)
			return StatusCode(StatusCodes.Status401Unauthorized, new AuthModel(null, Url));


		if (email != User.Identity.Name && user.Roles != "ROLE_ADMIN")
			return StatusCode(StatusCodes.Status403Forbidden, "You don't have enough permissions");

		User? targetUser = await _userService.GetByEmail(email);

		if (targetUser == null)
			return StatusCode(StatusCodes.Status404NotFound, "User not found");

		targetUser.Username = form.Username;
		targetUser.Firstname = form.Firstname;
		targetUser.Name = form.Name;
		targetUser.Password = _passwordHash.Hash(form.Password);

		var userUpdated = await _userService.Update(targetUser);
		return Ok(new UserModel(userUpdated, Url));
	}

	/// <summary>
	/// Modification d'un utilisateur
	/// </summary>
	/// <param name="email">Adresse mail de l'utilisaur cible</param>
	/// <returns>Un utilisateur</returns>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	/// <response code="403">Si vous n'êtes pas l'utilisateur cible ou un administrateur.</response>
	/// <response code="404">Si l'utilisateur cible n'existe pas.</response>
	[HttpPatch, Route("{email}")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update([FromBody] UserPatchModel form, string email)
	{
		if (User.Identity == null || User.Identity.Name == null)
			return StatusCode(401, new AuthModel(null, Url));

		var user = await _userService.GetUserAsync(User);
		if (user == null)
			return StatusCode(StatusCodes.Status401Unauthorized, new AuthModel(null, Url));


		if (email != user.Email && user.Roles != "ROLE_ADMIN")
			return StatusCode(StatusCodes.Status403Forbidden, "You don't have enough permissions");

		User? targetUser = await _userService.GetByEmail(email);

		if (targetUser == null)
			return StatusCode(StatusCodes.Status404NotFound, "User not found");

		if (!String.IsNullOrWhiteSpace(form.Username))
		{
			if (form.Username.Length < 3)
				return StatusCode(StatusCodes.Status403Forbidden, "The field Username must be a string a minimum length of '3'.");
			targetUser.Username = form.Username;
		}
		if (!String.IsNullOrWhiteSpace(form.Firstname))
		{
			if (form.Firstname.Length < 3)
				return StatusCode(StatusCodes.Status403Forbidden, "The field Firstname must be a string a minimum length of '3'.");
			targetUser.Firstname = form.Firstname;
		}
		if (!String.IsNullOrWhiteSpace(form.Name))
		{
			if (form.Name.Length < 3)
				return StatusCode(StatusCodes.Status403Forbidden, "The field Name must be a string a minimum length of '3'.");
			targetUser.Name = form.Name;
		}
		if (!String.IsNullOrWhiteSpace(form.Password))
		{
			if (form.Password.Length < 8)
				return StatusCode(StatusCodes.Status403Forbidden, "The field Password must be a string a minimum length of '8'.");
			targetUser.Password = _passwordHash.Hash(form.Password);
		}

		var userUpdated = await _userService.Update(user);
		return Ok(new UserModel(userUpdated, Url));
	}


	/// <summary>
	/// Recupération de l'avatar de l'utilisateur courant
	/// </summary>
	/// <see cref="GetAvatar(String)"/>
	/// <returns>L'avatar de l'utilisateur courant</returns>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	[HttpGet]
	[Authorize]
	[Route("avatar")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> GetAvatar()
	{
		var email = User.Identity?.Name;
		if (email == null)
			return Unauthorized();
		return await GetAvatar(email);
	}

	
	/// <summary>
	/// Récupération d'un avatar d'un utilisateur
	/// </summary>
	/// <param name="guid">Identifiant d'un utilisateur cible</param>
	/// <returns>Avatar de l'utilisateur</returns>
	/// <response code="404">Si l'utilisateur visé n'existe pas ou qu'il ne dispose pas d'avatar.</response>
	[HttpGet]
	[AllowAnonymous]
	[Route("avatar/{email}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetAvatar(String email)
	{
		var targetUser = await _userService.GetByEmail(email);
		if (targetUser == null || targetUser.Avatar == null || targetUser.AvatarContentType == null)
			return NotFound();
		return File(targetUser.Avatar, targetUser.AvatarContentType);
	}

	/// <summary>
	/// Mie à jour de l'avatar de l'utilisateur courant
	/// </summary>
	/// <see cref="UploadAvatar(string,Microsoft.AspNetCore.Http.IFormFile?)"/>
	/// <returns></returns>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	[HttpPut]
	[Authorize]
	[Route("avatar")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public Task<IActionResult> UploadAvatar(IFormFile? form = null)
	{
		return UploadAvatar( User.Identity?.Name ?? String.Empty,form);
	}


	/// <summary>
	/// Mettre à jour l'avatar d'un utilisateur
	/// </summary>
	/// <param name="email">Identifiant de l'utilisateur cible</param>
	/// <param name="form">Informations sur le nouvel avatar (null si l'on souhaite supprimer celui courant)</param>
	/// <returns></returns>
	/// <response code="404">Si l'utilisateur visée n'existe pas.</response>
	/// <response code="403">Vous ne disposez pas des droits suffisant.</response>
	/// <response code="401">Si vous n'êtes pas authentifié.</response>
	[HttpPut]
	[Authorize]
	[Route("avatar/{email}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> UploadAvatar(string email, IFormFile? form = null)
	{

		var user = await _userService.GetUserAsync(User);
		if (user == null)
			return StatusCode(401, new AuthModel(null, Url));

		var targetUser = String.IsNullOrWhiteSpace(email) ? user : await _userService.GetByEmail(email);
		if (targetUser == null)
			return NotFound();
		if (user.Email != targetUser.Email && user.Roles != "ROLE_ADMIN")
			return Forbid();


		if (form == null)
		{
			targetUser.Avatar = null;
			targetUser.AvatarContentType = null;
		}
		else
		{
			using (var memoryStream = new MemoryStream())
			{
				await form.CopyToAsync(memoryStream);
				targetUser.Avatar = memoryStream.ToArray();
			}
			targetUser.AvatarContentType = form.ContentType;
		}


		await _userService.Update(targetUser);

		return Ok();
	}
}