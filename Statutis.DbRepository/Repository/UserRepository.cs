using Microsoft.EntityFrameworkCore;
using Statutis.Core.Interfaces.DbRepository;
using Statutis.Entity;

namespace Statutis.DbRepository.Repository;

public class UserRepository : IUserRepository
{
    private readonly StatutisContext _ctx;

    public UserRepository(StatutisContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<List<User>> GetAll()
    {
        return await _ctx.User.ToListAsync();
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _ctx.User.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User?> GetByUsername(string username)
    {
        return await _ctx.User.FirstOrDefaultAsync(x => x.Username == username);
    }

    public async Task<User> Update(User user)
    {
        _ctx.Update(user);
        await _ctx.SaveChangesAsync();
        return user;
    }

    public async Task Delete(User user)
    {
        _ctx.Remove(user);
        await _ctx.SaveChangesAsync();
    }
}