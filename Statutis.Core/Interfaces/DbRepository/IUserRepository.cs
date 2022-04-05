using Statutis.Entity;

namespace Statutis.Core.Interfaces.DbRepository;

public interface IUserRepository
{
    //Get
    List<User> GetAll();
    User GetByEmail(string email);
    User GetByUsername(string email);
    
    //Update
    User Update(User user);
    
    //delete
    void Delete(User user);
}