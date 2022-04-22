using Statutis.Core.Interfaces.Business.Service;
using Statutis.Core.Interfaces.DbRepository.Service;
using Statutis.Entity;
using Statutis.Entity.Service;

namespace Statutis.Business;

public class GroupService : IGroupService
{
	private IGroupRepository _repository;

	public GroupService(IGroupRepository repository)
	{
		_repository = repository;
	}

	public Task<List<Group>> GetAll()
	{
		return _repository.GetAll();
	}

	public Task<Group?> Get(Guid guid)
	{
		return _repository.Get(guid);
	}

	public Task<List<Group>> Get(string name)
	{
		return _repository.Get(name);
	}

	public Task<Group> Insert(Group group)
	{
		return _repository.Insert(group);
	}

	public Task<Group> Update(Group group)
	{
		return _repository.Update(group);
	}

	public async Task Delete(Group group)
	{
		await _repository.Delete(group);
	}

	public Task<List<Group>> GetPublicGroup()
	{
		return _repository.GetPublicGroup();
	}

	public Task<List<Group>> GetFromUser(User user)
	{
		return _repository.GetFromUser(user);
	}
}