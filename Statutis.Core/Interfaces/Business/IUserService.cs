using Statutis.Entity;

namespace Statutis.Core.Interfaces.Business;

public interface IUserService
{
    //Get
    Task<List<User>> GetAll();
    Task<User> GetByEmail(string email);
    Task<User> GetByUsername(string email);
    
    //Update
    Task<User> Update(User user);
    
    //delete
    Task Delete(User user);
}