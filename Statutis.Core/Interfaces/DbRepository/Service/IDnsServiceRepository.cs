using Statutis.Entity.Service.Check;

namespace Statutis.Core.Interfaces.DbRepository.Service;

public interface IDnsServiceRepository
{
    //Select
    List<DnsService> SelectAll();
    DnsService Select(Guid serviceId);
    List<DnsService> Select(string name);
    List<DnsService> SelectByHostname(string hostname);
    
    //Update
    DnsService Update(DnsService sshService);
    
    //Insert
    DnsService Insert(DnsService sshService);
    
    //Delete
    void Delete(DnsService sshService);
}