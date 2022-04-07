using Statutis.Entity.Service.Check;

namespace Statutis.Core.Interfaces.DbRepository.Service;

public interface IDnsServiceRepository
{
    //Select
    Task<List<DnsService>> SelectAll();
    Task<DnsService> Select(Guid serviceId);
    Task<List<DnsService>> Select(string name);
    Task<List<DnsService>> SelectByHostname(string hostname);
    
    //Update
    Task<DnsService> Update(DnsService sshService);
    
    //Insert
    Task<DnsService> Insert(DnsService sshService);
    
    //Delete
    Task Delete(DnsService sshService);
}