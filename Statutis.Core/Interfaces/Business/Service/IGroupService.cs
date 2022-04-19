using Statutis.Entity.Service;

namespace Statutis.Core.Interfaces.Business.Service;

public interface IGroupService
{
    //Get
    Task<List<Entity.Service.Group>> GetAll();
    Task<Entity.Service.Group> Get(Guid guid);
    Task<List<Entity.Service.Group>> Get(string name);
    
    //Insert
    Task<Entity.Service.Group> Insert(Entity.Service.Group @group);
    
    //Update
    Task<Entity.Service.Group> Update(Entity.Service.Group @group);
    
    //Delete
    Task Delete(Entity.Service.Group @group);

    public Task<List<Group>> GetPublicGroup();

}