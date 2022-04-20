using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Statutis.API.Models;
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
            return Unauthorized(new LoginModel(null, false, "E-Mail not found", Url));
        }

        //Check password
        if (!_passwordHash.Verify(user.Password, form.Password))
        {
            return Unauthorized(new LoginModel(null, false, "Password doesn't match", Url));
        }

        string token = _authService.GenerateToken(user.Email, new []{user.Roles});

        return Ok(new LoginModel(token, true, null, Url));
    }

    [HttpPost]
    [Authorize]
    [Route("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginModel))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(LoginModel))]
    public async Task<IActionResult> Refresh()
    {
        if (User.Identity == null || User.Identity.Name == null)
            return Unauthorized(new LoginModel(null, false, "Identity null", Url));
        
        var user = await _userService.GetByEmail(User.Identity.Name);
        if (user == null)
        {
            return Unauthorized(new LoginModel(null, false, "Username not found", Url));
        }

        string newToken = _authService.GenerateToken(user.Username, new [] {user.Roles});
        
        return Ok(new LoginModel(newToken, true, null, Url));
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationForm registrationForm)
    {
        var res = await _authService.Registration(registrationForm.Username, registrationForm.Password, registrationForm.Email);

        if (!res.Item2)
            return StatusCode(406, new AuthModel(res.Item1, Url));
        
        
        return Ok(new AuthModel(null, Url));
    }

}