using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Get()
    {
        if (User.Identity == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new AuthModel(null, Url));
        
        string email = User.Identity.Name;
        
        if (email == null)
            return StatusCode(StatusCodes.Status404NotFound, new AuthModel(null, Url));

        User user = await _userService.GetByEmail(email);
        if(user == null)
            return StatusCode(StatusCodes.Status404NotFound, new AuthModel(null, Url));
        
        return Ok(user);
    }

    [HttpGet("email/{email}")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> GetByEmail([Required]string email)
    {
        if (User.Identity == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new AuthModel(null, Url));
        if (email == "")
            return StatusCode(StatusCodes.Status400BadRequest, "Query parameter email is required !");    
        User user = await _userService.GetByEmail(email);

        return Ok(user);
    }
    
    [HttpGet("username/{username}")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> GetByUsername([Required]string username)
    {
        if (User.Identity == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new AuthModel(null, Url));
        if (username == "")
            return StatusCode(StatusCodes.Status400BadRequest, "Query parameter email is required !");    
        User user = await _userService.GetByUsername(username);

        return Ok(user);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> update([FromBody] RegistrationForm form)
    {
        if (User.Identity == null || User.Identity.Name == null)
            return StatusCode(401, new AuthModel(null, Url));

        var user = await _userService.GetByEmail(User.Identity.Name);
        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new AuthModel(null, Url));


        if (form.Email != User.Identity.Name && user.Roles != "ROLE_ADMIN")
            return StatusCode(StatusCodes.Status403Forbidden, "You don't have enough permissions");

        user = await _userService.GetByEmail(form.Email);

        if (user == null)
            return StatusCode(StatusCodes.Status404NotFound, "User not found");
        
        user.Username = form.Username;
        user.Password = _passwordHash.Hash(form.Password);

        var userUpdated = await _userService.Update(user);
        return Ok(userUpdated);
    }
}