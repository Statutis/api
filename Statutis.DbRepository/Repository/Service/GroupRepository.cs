using Microsoft.EntityFrameworkCore;
using Statutis.Core.Interfaces.DbRepository.Service;
using Statutis.Entity;
using Statutis.Entity.Service;

namespace Statutis.DbRepository.Repository.Service;

public class GroupRepository : IGroupRepository
{

	private readonly StatutisContext _ctx;

	public GroupRepository(StatutisContext ctx)
	{
		_ctx = ctx;
	}


	public async Task<List<Group>> GetAll()
	{
		return await _ctx.Group.Include(x => x.Services).Include(x => x.Teams).ToListAsync();
	}

	public async Task<Group?> Get(Guid guid)
	{
		return await _ctx.Group
			.Include(x => x.Services)
			.Include(x => x.Teams)
			.FirstOrDefaultAsync(x => x.GroupId == guid);
	}

	public async Task<List<Group>> GetFromUser(User user)
	{
		var TeamId = user.Teams.Select(x => x.TeamId);

		return await _ctx.Group.AsQueryable()
			.Include(x => x.Services)
			.Where(x => x.Teams.Any(y => TeamId.Contains(y.TeamId)))
			.ToListAsync();
	}



	public async Task<List<Group>> Get(string name)
	{
		return await _ctx.Group.Where(x => x.Name == name).ToListAsync();
	}

	public async Task<List<Group>> GetByTeamId(Guid teamGuid)
	{
		List<Group> groups = await _ctx.Group.Where(x => x.Teams.Select(y => y.TeamId).Contains(teamGuid)).ToListAsync();

		return groups;
	}

	public async Task<Group> Insert(Group @group)
	{
		_ctx.Group.Add(group);
		await _ctx.SaveChangesAsync();
		return group;
	}

	public async Task<Group> Update(Group @group)
	{
		_ctx.Group.Update(group);
		await _ctx.SaveChangesAsync();
		return group;
	}

	public async Task Delete(Group @group)
	{
		_ctx.Group.Remove(group);
		await _ctx.SaveChangesAsync();
	}

	public Task<List<Group>> GetPublicGroup()
	{
		return _ctx.Group
			.Where(x => x.IsPublic)
			.Include(x => x.Services)
			.Include(x => x.Teams)
			.ToListAsync();
	}
}