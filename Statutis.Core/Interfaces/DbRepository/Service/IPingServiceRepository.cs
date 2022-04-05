using Statutis.Entity.Service.Check;

namespace Statutis.Core.Interfaces.DbRepository.Service;

public interface IPingServiceRepository
{
    //Select
    List<PingService> SelectAll();
    PingService Select(Guid serviceId);
    List<PingService> Select(string name);
    List<PingService> SelectByHostname(string hostname);
    
    //Update
    PingService Update(PingService sshService);
    
    //Insert
    PingService Insert(PingService sshService);
    
    //Delete
    void Delete(PingService sshService);
}