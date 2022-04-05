using Statutis.Entity.Service;

namespace Statutis.Core.Interfaces.DbRepository.Service;

public interface IServiceService
{
    //Get
    List<Entity.Service.Service> GetAll();
    List<Entity.Service.Service> GetByHost(string host);
    List<Entity.Service.Service> GetByServiceType(ServiceType serviceType);
    
    Entity.Service.Service Get(string name);
    Entity.Service.Service Get(Guid guid);
    
    //insert
    Entity.Service.Service Insert(Entity.Service.Service service);
    
    //Update
    Entity.Service.Service Update(Entity.Service.Service service);
    
    //Delete
    void Delete(Entity.Service.Service delete);
}