using System.Security.Claims;
using Statutis.Core.Interfaces.Business;
using Statutis.Core.Interfaces.DbRepository;
using Statutis.Entity;

namespace Statutis.Business;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<List<User>> GetAll()
	{
		return _userRepository.GetAll();
	}

	public Task<User?> GetByEmail(string email)
	{
		return _userRepository.GetByEmail(email);
	}

	public Task<User?> GetByUsername(string email)
	{
		return _userRepository.GetByUsername(email);
	}

	public Task<bool> Insert(User user)
	{
		return _userRepository.Insert(user);
	}

	public async Task<User> Update(User user)
	{
		return await _userRepository.Update(user);
	}

	public async Task<bool> IsUserInTeam(string email, Team team)
    {
        var user = await _userRepository.GetByEmail(email);
        if (user == null)
            return false;
        return user.Teams.Contains(team);
    }
    
    public async Task<bool> IsUserInTeam(User user, List<Team> team)
    {
        foreach (Team userTeam in user.Teams)
        {
            if (team.Contains(userTeam))
            {
                return true;
            }
        };
        return false;
    }
	public Task Delete(User user)
	{
		return _userRepository.Delete(user);
	}

	public Task<User?> GetUserAsync(ClaimsPrincipal principal)
	{
		return _userRepository.GetByEmail(principal.FindFirstValue(ClaimTypes.Name));
	}
}