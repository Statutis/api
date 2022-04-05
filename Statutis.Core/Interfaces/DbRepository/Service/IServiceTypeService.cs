namespace Statutis.Core.Interfaces.DbRepository.Service;

public interface IServiceTypeService
{
    //get
    List<Entity.Service.ServiceType> GetAll();

    Entity.Service.ServiceType Get(string name);
    
    //insert
    Entity.Service.ServiceType Insert(Entity.Service.ServiceType serviceType);
    
    //Update
    Entity.Service.ServiceType Update(Entity.Service.ServiceType serviceType);
    
    //Delete
    Entity.Service.ServiceType Delete(Entity.Service.ServiceType serviceType);

}