namespace Statutis.Core.Interfaces.DbRepository.Service;

public interface IGroupRepository
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
    Task<Entity.Service.Group> Delete(Entity.Service.Group @group);

}