namespace Statutis.Core.Interfaces.DbRepository.Service;

public interface IGroupService
{
    //Get
    List<Entity.Service.Group> GetAll();
    Entity.Service.Group Get(Guid guid);
    List<Entity.Service.Group> Get(string name);
    
    //Insert
    Entity.Service.Group Insert(Entity.Service.Group @group);
    
    //Update
    Entity.Service.Group Update(Entity.Service.Group @group);
    
    //Delete
    Entity.Service.Group Delete(Entity.Service.Group @group);

}