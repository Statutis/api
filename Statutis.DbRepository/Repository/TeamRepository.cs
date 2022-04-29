using Microsoft.EntityFrameworkCore;
using Statutis.Core.Interfaces.DbRepository;
using Statutis.Entity;

namespace Statutis.DbRepository.Repository;

public class TeamRepository : ITeamRepository
{
	private readonly StatutisContext _ctx;

	public TeamRepository(StatutisContext ctx)
	{
		_ctx = ctx;
	}

	public async Task<List<Team>> GetAll()
	{
		return await _ctx.Team.Include(x => x.Groups).Include(x => x.Users).ToListAsync();
	}

	public async Task<Team?> Get(Guid guid)
	{
		return await _ctx.Team.Include(x => x.Groups).Include(x => x.Users).FirstOrDefaultAsync(x => x.TeamId == guid);
	}

	public async Task<Team?> Get(string name)
	{
		return await _ctx.Team.FirstOrDefaultAsync(x => x.Name == name);
	}

	public async Task<List<Team>> GetTeamsOfUser(User user)
	{
		return await _ctx.Team.Where(x => x.Users.Contains(user)).ToListAsync();
	}

	public async Task<Team> Update(Team team)
	{
		_ctx.Team.Update(team);
		await _ctx.SaveChangesAsync();
		return team;
	}

	public async Task Add(Team team)
	{
		_ctx.Team.Add(team);
		await _ctx.SaveChangesAsync();
	}

	public async Task Delete(Team team)
	{
		_ctx.Team.Remove(team);
		await _ctx.SaveChangesAsync();
	}

	public Task<List<Team>> GetAllPublic()
	{
		return _ctx.Team
			.Include(x => x.Groups).Include(x => x.Users)
			.Where(x => x.Groups.Any(y => y.IsPublic)).ToListAsync();
	}
}