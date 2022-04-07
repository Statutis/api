using Statutis.Entity.Service.Check;

namespace Statutis.Core.Interfaces.DbRepository.Service;

public interface IHttpServiceRepository
{
    //Select
    Task<List<HttpService>> SelectAll();
    Task<HttpService?> Select(Guid serviceId);
    Task<List<HttpService>> Select(string name);
    Task<List<HttpService>> SelectByHostname(string hostname);
    
    //Update
    Task<HttpService> Update(HttpService sshService);
    
    //Insert
    Task<HttpService> Insert(HttpService sshService);
    
    //Delete
    Task Delete(HttpService sshService);
}