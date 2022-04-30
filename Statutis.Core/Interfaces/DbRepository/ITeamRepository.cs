using Statutis.Entity;

namespace Statutis.Core.Interfaces.DbRepository;

public interface ITeamRepository
{
    //Get
    Task<List<Team>> GetAll();
    Task<Team?> Get(Guid guid);
    Task<Team?> Get(string name);

    Task<List<Team>> GetTeamsOfUser(User user);
    
    //Update
    Task<Team> Update(Team team);
    
    Task Add(Team team);
    
    //delete
    Task Delete(Team team);

    public Task<List<Team>> GetAllPublic();
}