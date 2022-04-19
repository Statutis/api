namespace Statutis.Core.Interfaces.Business;

public interface IAuthService
{
    public string GenerateToken(string username, string[] roles);
}