using Statutis.Entity.Service.Check;

namespace Statutis.Core.Interfaces.DbRepository.Service;

public interface IPingServiceRepository
{
    //Select
    Task<List<PingService>> SelectAll();
    Task<PingService?> Select(Guid serviceId);
    Task<List<PingService>> Select(string name);
    Task<List<PingService>> SelectByHostname(string hostname);
    
    //Update
    Task<PingService> Update(PingService sshService);
    
    //Insert
    Task<PingService> Insert(PingService sshService);
    
    //Delete
    Task Delete(PingService sshService);
}