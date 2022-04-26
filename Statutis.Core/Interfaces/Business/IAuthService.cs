namespace Statutis.Core.Interfaces.Business;

public interface IAuthService
{
    public string GenerateToken(string username, string[] roles);

    public Task<Tuple<string, bool>> Registration(string username, string password, string email, string? name = null, string? firstname = null);

}