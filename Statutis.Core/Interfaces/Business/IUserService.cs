using System.Security.Claims;
using Statutis.Entity;

namespace Statutis.Core.Interfaces.Business;

public interface IUserService
{
    //Get
    Task<List<User>> GetAll();
    Task<User?> GetByEmail(string email);
    Task<User?> GetByUsername(string email);

    Task<bool> Insert(User user);
    
    //Update
    Task<User> Update(User user);
    
    //delete
    Task Delete(User user);

    public Task<User?> GetUserAsync(ClaimsPrincipal principal);

    Task<bool> IsUserInTeam(string email, Team team);
    bool IsUserInTeam(User user, List<Team> team);

    Task<bool> IsUserInGroup(User user, Guid guid);
}