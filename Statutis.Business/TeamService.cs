using Statutis.Core.Interfaces.Business;
using Statutis.Core.Interfaces.DbRepository;
using Statutis.Core.Interfaces.DbRepository.Service;
using Statutis.Entity;

namespace Statutis.Business;

public class TeamService : ITeamService
{

	private ITeamRepository _teamRepository;
	private readonly IGroupRepository _groupRepository;

	public TeamService(ITeamRepository teamRepository, IGroupRepository _groupRepository)
	{
		_teamRepository = teamRepository;
		this._groupRepository = _groupRepository;
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

	public Task Add(Team team)
	{
		return _teamRepository.Add(team);
	}

	public Task<Team> Update(Team team)
	{
		return _teamRepository.Update(team);
	}

	public Task Delete(Team team)
	{
		return _teamRepository.Delete(team);
	}

	public async Task<bool> IsPublic(Team team)
	{

		return (await _groupRepository.GetByTeamId(team.TeamId)).Any(x => x.IsPublic);
	}

	public Task<List<Team>> GetAllPublic()
	{
		return _teamRepository.GetAllPublic();
	}


}