using Statutis.Entity.Service.Check;

namespace Statutis.Core.Interfaces.DbRepository.Service;

public interface IHttpServiceRepository
{
    //Select
    List<HttpService> SelectAll();
    HttpService Select(Guid serviceId);
    List<HttpService> Select(string name);
    List<HttpService> SelectByHostname(string hostname);
    
    //Update
    HttpService Update(HttpService sshService);
    
    //Insert
    HttpService Insert(HttpService sshService);
    
    //Delete
    void Delete(HttpService sshService);
}