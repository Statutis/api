namespace Statutis.Core.Interfaces.DbRepository.Service;

public interface IServiceTypeRepository
{
    //get
    Task<List<Entity.Service.ServiceType>> GetAll();

    Task<Entity.Service.ServiceType> Get(string name);
    
    //insert
    Task<Entity.Service.ServiceType> Insert(Entity.Service.ServiceType serviceType);
    
    //Update
    Task<Entity.Service.ServiceType> Update(Entity.Service.ServiceType serviceType);
    
    //Delete
    Task<Entity.Service.ServiceType> Delete(Entity.Service.ServiceType serviceType);

}