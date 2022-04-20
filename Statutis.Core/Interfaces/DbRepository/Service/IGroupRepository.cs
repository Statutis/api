using Statutis.Entity.Service;

namespace Statutis.Core.Interfaces.DbRepository.Service;

public interface IGroupRepository
{
    //Get
    Task<List<Entity.Service.Group>> GetAll();
    Task<Entity.Service.Group?> Get(Guid guid);
    Task<List<Entity.Service.Group>> Get(string name);
    
    //Insert
    Task<Entity.Service.Group> Insert(Entity.Service.Group @group);
    
    //Update
    Task<Entity.Service.Group> Update(Entity.Service.Group @group);
    
    //Delete
    Task Delete(Entity.Service.Group @group);

    /// <summary>
    /// Touts les groupes qui contiennent au moins un service public
    /// </summary>
    /// <returns></returns>
    public Task<List<Group>> GetPublicGroup();

}