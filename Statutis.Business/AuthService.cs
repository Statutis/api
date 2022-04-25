using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Statutis.Core.Interfaces.Business;
using Statutis.Entity;

namespace Statutis.Business;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    private readonly IPasswordHash _passwordHash;

    public AuthService(IConfiguration configuration, IUserService userService, IPasswordHash passwordHash)
    {
        _configuration = configuration;
        _userService = userService;
        _passwordHash = passwordHash;
    }

    public string GenerateToken(string username, string[] roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWT:secret"));
        var expirationHours = int.Parse(_configuration.GetValue<string>("JWT:expiration_hour"));

        var tokenDescription = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, string.Join(",", roles))
            }),
            Expires = DateTime.UtcNow.AddHours(expirationHours),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        var token = tokenHandler.CreateToken(tokenDescription);
        return tokenHandler.WriteToken(token);
    }

    public async Task<Tuple<string, bool>> Registration(string username, string password, string email, string? name = null, string? firstname = null)
    {
        //Check if username exists
        var isUsernameNull = (await _userService.GetByUsername(username) == null);
        if (!isUsernameNull)
            return new Tuple<string, bool>("Username already taken", false);
        //check if email exists
        var isEmailNull = (await _userService.GetByEmail(email) == null);
        if (!isEmailNull)
            return new Tuple<string, bool>("Email already taken", false);

        User user = new User(email, username, _passwordHash.Hash(password)){Roles = "ROLE_USER"};
        user.Name = name;
        user.Firstname = firstname;
        bool status = await _userService.Insert(user);

        return new Tuple<string, bool>("", status);

    }
}