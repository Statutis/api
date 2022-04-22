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

	public Task Delete(User user)
	{
		return _userRepository.Delete(user);
	}

	public Task<User?> GetUserAsync(ClaimsPrincipal principal)
	{
		return _userRepository.GetByEmail(principal.FindFirstValue(ClaimTypes.Name));
	}
}