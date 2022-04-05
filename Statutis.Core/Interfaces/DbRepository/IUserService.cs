using Statutis.Entity;

namespace Statutis.Core.Interfaces.DbRepository;

public interface IUserService
{
    //Get
    List<User> GetAll();
    User Get(int id);
    User GetByEmail(string email);
    User GetByUsername(string email);
    
    //Update
    User Update(User user);
    
    //delete
    void Delete(User user);
}