using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Statutis.API.Form;
using Statutis.API.Models;
using Statutis.Core.Interfaces.Business;

namespace Statutis.API.Controllers;

/// <summary>
/// Controleur sur l'identification
/// </summary>
[Tags("Authentification")]
[ApiController]
[Route("api/auth")]
public class AuthenticateController : Controller
{
    private readonly IPasswordHash _passwordHash;
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    
    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="passwordHash"></param>
    /// <param name="userService"></param>
    /// <param name="authService"></param>
    public AuthenticateController(IPasswordHash passwordHash, IUserService userService, IAuthService authService)
    {
        _passwordHash = passwordHash;
        _userService = userService;
        _authService = authService;
    }

    
    /// <summary>
    /// Connection d'un utilisateur
    /// </summary>
    /// <param name="form">Formulaire de connexion</param>
    /// <returns>Token</returns>
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

    
    /// <summary>
    /// Rafraichissement d'un token
    /// </summary>
    /// <returns>Nouveau token</returns>
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

        string newToken = _authService.GenerateToken(user.Email, new [] {user.Roles});
        
        return Ok(new LoginModel(newToken, true, null, Url));
    }
    
    
    /// <summary>
    /// Enregistrement d'un nouvel utilisateur
    /// </summary>
    /// <param name="registrationForm">Informations sur ce nouvel utilisateur</param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationForm registrationForm)
    {
        var res = await _authService.Registration(registrationForm.Username, registrationForm.Password, registrationForm.Email, registrationForm.Name, registrationForm.Firstname);

        if (!res.Item2)
            return StatusCode(406, new AuthModel(res.Item1, Url));
        
        
        return Ok(new AuthModel(null, Url));
    }

}