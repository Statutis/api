using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Statutis.API.Models;
using Statutis.Core.Interfaces.Business;
using Statutis.Entity;

namespace Statutis.API.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UserController : Controller
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        if (User.Identity == null)
            return StatusCode(401, new AuthModel(null, Url));
        
        string email = User.Identity.Name;
        
        if (email == null)
            return StatusCode(401, new AuthModel(null, Url));

        User user = await _userService.GetByEmail(email);
        if(user == null)
            return StatusCode(500, new AuthModel(null, Url));
        
        return Ok(user);
    }
}