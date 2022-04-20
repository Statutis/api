using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Statutis.Core.Interfaces.Business;

namespace Statutis.Business;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    
    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
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
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        var token = tokenHandler.CreateToken(tokenDescription);
        return tokenHandler.WriteToken(token);
    }
}