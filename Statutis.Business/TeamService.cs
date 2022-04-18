using Statutis.Core.Interfaces.Business;
using Statutis.Core.Interfaces.DbRepository;
using Statutis.Entity;

namespace Statutis.Business;

public class TeamService : ITeamService
{

	private ITeamRepository _teamRepository;

	public TeamService(ITeamRepository teamRepository)
	{
		_teamRepository = teamRepository;
	}

	public Task<List<Team>> GetAll()
	{
		return _teamRepository.GetAll();
	}

	public Task<Team?> Get(Guid guid)
	{
		return _teamRepository.Get(guid);
	}

	public Task<Team?> Get(string name)
	{
		return _teamRepository.Get(name);
	}

	public Task<List<Team>> GetTeamsOfUser(User user)
	{
		return _teamRepository.GetTeamsOfUser(user);
	}

	public Task<Team> Update(Team team)
	{
		return _teamRepository.Update(team);
	}

	public Task Delete(Team team)
	{
		return _teamRepository.Delete(team);
	}
}