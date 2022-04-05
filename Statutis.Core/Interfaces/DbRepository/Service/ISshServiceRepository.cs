using Statutis.Entity.Service.Check;

namespace Statutis.Core.Interfaces.DbRepository.Service;

public interface ISshServiceRepository
{
    //Select
    List<SshService> SelectAll();
    SshService Select(Guid serviceId);
    List<SshService> Select(string name);
    List<SshService> SelectByHostname(string hostname);
    
    //Update
    SshService Update(SshService sshService);
    
    //Insert
    SshService Insert(SshService sshService);
    
    //Delete
    void Delete(SshService sshService);
}