using Statutis.Entity;

namespace Statutis.Core.Interfaces.Business;

public interface ITeamService
{
	//Get
	Task<List<Team>> GetAll();
	Task<List<Team>> GetAllPublic();

	Task<Team?> Get(Guid guid);

	Task<Team?> Get(string name);

	Task<List<Team>> GetTeamsOfUser(User user);

	//Update
	Task Add(Team team);


	Task<Team> Update(Team team);

	//delete
	Task Delete(Team team);

	Task<bool> IsPublic(Team team);
}