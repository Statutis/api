using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Statutis.API.Form;
using Statutis.API.Models;
using Statutis.Core.Form;
using Statutis.Core.Interfaces.Business;
using Statutis.Entity;

namespace Statutis.API.Controllers;

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

	[HttpGet("me")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Get()
	{
		if (User.Identity == null)
			return StatusCode(StatusCodes.Status401Unauthorized, new AuthModel(null, Url));

		string? email = User.Identity.Name;

		if (email == null)
			return StatusCode(StatusCodes.Status404NotFound, new AuthModel(null, Url));

		User? user = await _userService.GetByEmail(email);
		if (user == null)
			return StatusCode(StatusCodes.Status404NotFound, new AuthModel(null, Url));

		return Ok(new UserModel(user, Url));
	}

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

	[HttpGet("username/{username}")]
	[Authorize()]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetByUsername([Required] string username)
	{

		if (username == "")
			return StatusCode(StatusCodes.Status400BadRequest, "Query parameter email is required !");
		User? user = await _userService.GetByUsername(username);
		if (user == null)
			return NotFound();

		return Ok(new UserModel(user, Url));
	}

	[HttpGet("")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserModel>))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetAll()
	{

		List<User> users = await _userService.GetAll();

		return Ok(users.Select(x => new UserModel(x, Url)));
	}


	[HttpPut, Route("{email}")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserPutModel))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update([FromBody] UserPutModel form, String email)
	{
		if (User.Identity == null || User.Identity.Name == null)
			return StatusCode(401, new AuthModel(null, Url));

		var user = await _userService.GetByEmail(User.Identity.Name);
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

	[HttpGet]
	[Authorize]
	[Route("avatar")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetAvatar()
	{
		var email = User.Identity?.Name;
		if (email == null)
			return Forbid();
		return await GetAvatar(email);
	}

	[HttpGet]
	[AllowAnonymous]
	[Route("avatar/{email}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetAvatar(String email)
	{
		var targetUser = await _userService.GetByEmail(email);
		if (targetUser.Avatar == null || targetUser.AvatarContentType == null)
			return NotFound();
		return File(targetUser.Avatar, targetUser.AvatarContentType);
	}

	[HttpPut]
	[Authorize]
	[Route("avatar")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public Task<IActionResult> UploadAvatar(IFormFile? form = null)
	{
		return UploadAvatar(form, User.Identity?.Name);
	}

	[HttpPut]
	[Authorize]
	[Route("avatar/{email}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> UploadAvatar(IFormFile? form = null, string? email = null)
	{

		var user = await _userService.GetUserAsync(User);
		if (user == null)
			return StatusCode(401, new AuthModel(null, Url));

		var targetUser = String.IsNullOrWhiteSpace(email) ? user : await _userService.GetByEmail(email);
		if (targetUser == null || (user.Email != targetUser.Email && user.Roles != "ROLE_ADMIN"))
			return NotFound();


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