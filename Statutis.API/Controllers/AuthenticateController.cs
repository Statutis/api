using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Statutis.API.Models;
using Statutis.Business;
using Statutis.Core.Form;
using Statutis.Core.Interfaces.Business;

namespace Statutis.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticateController : Controller
{
    private readonly IPasswordHash _passwordHash;
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    
    public AuthenticateController(IPasswordHash passwordHash, IUserService userService, IAuthService authService)
    {
        _passwordHash = passwordHash;
        _userService = userService;
        _authService = authService;
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginModel))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(LoginModel))]
    public async Task<IActionResult> Login([FromBody] AuthenticationForm form)
    {
        var user = await _userService.GetByEmail(form.Username);
        if (user == null)
        {
            return Unauthorized(new LoginModel(null, false, "E-Mail not found", Url.Action("Login", "Authenticate"),
                Url.Action("Refresh", "Authenticate")));
        }

        //Check password
        if (!_passwordHash.Verify(user.Password, form.Password))
        {
            return Unauthorized(new LoginModel(null, false, "Password doesn't match",
                Url.Action("Login", "Authenticate"), Url.Action("Refresh", "Authenticate")));
        }

        string token = _authService.GenerateToken(user.Email, new string[] {user.Roles});

        return Ok(new LoginModel(token, true, null, Url.Action("Login", "Authenticate"),
            Url.Action("Refresh", "Authenticate")));
    }

    [HttpPost]
    [Authorize]
    [Route("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginModel))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(LoginModel))]
    public async Task<IActionResult> Refresh()
    {
        if (User.Identity == null || User.Identity.Name == null)
            return Unauthorized(new LoginModel(null, false, "Identity null", Url.Action("Login", "Authenticate"),
                Url.Action("Refresh", "Authenticate")));
        
        var user = await _userService.GetByEmail(User.Identity.Name);
        if (user == null)
        {
            return Unauthorized(new LoginModel(null, false, "Username not found", Url.Action("Login", "Authenticate"),
                Url.Action("Refresh", "Authenticate")));
        }

        string new_token = _authService.GenerateToken(user.Username, new string[] {user.Roles});
        
        return Ok(new LoginModel(new_token, true, null, Url.Action("Login", "Authenticate"),
            Url.Action("Refresh", "Authenticate")));
    }

}