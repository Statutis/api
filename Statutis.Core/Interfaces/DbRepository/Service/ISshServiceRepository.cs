using Statutis.Entity.Service.Check;

namespace Statutis.Core.Interfaces.DbRepository.Service;

public interface ISshServiceRepository
{
    //Select
    Task<List<SshService>> SelectAll();
    Task<SshService> Select(Guid serviceId);
    Task<List<SshService>> Select(string name);
    Task<List<SshService>> SelectByHostname(string hostname);
    
    //Update
    Task<SshService> Update(SshService sshService);
    
    //Insert
    Task<SshService> Insert(SshService sshService);
    
    //Delete
    Task Delete(SshService sshService);
}