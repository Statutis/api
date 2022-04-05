using Statutis.Entity;

namespace Statutis.Core.Interfaces.Business;

public interface ITeamService
{
    //Get
    List<Team> GetAll();
    Team Get(Guid guid);
    Team Get(string name);

    List<Team> GetTeamsOfUser(User user);
    
    //Update
    Team Update(Team team);
    
    //delete
    void Delete(Team team);
}