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
    
    public async Task<List<User>> GetAll()
    {
        return await _userRepository.GetAll();
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _userRepository.GetByEmail(email);
    }

    public async Task<User?> GetByUsername(string email)
    {
        return await _userRepository.GetByUsername(email);
    }

    public async Task<User> Update(User user)
    {
        return await _userRepository.Update(user);
    }

    public async Task Delete(User user)
    {
        await _userRepository.Delete(user);
    }
}