using Microsoft.EntityFrameworkCore;
using Statutis.Core.Interfaces.DbRepository.Service;
using Statutis.Entity.Service;

namespace Statutis.DbRepository.Repository.Service;

public class ServiceRepository : IServiceRepository
{
    private readonly StatutisContext _ctx;

    public ServiceRepository(StatutisContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<List<Entity.Service.Service>> GetAll()
    {
        return await _ctx.Service.ToListAsync();
    }

    public async Task<List<Entity.Service.Service>> GetByHost(string host)
    {
        return await _ctx.Service.Where(x => x.Host == host).ToListAsync();
    }

    public async Task<List<Entity.Service.Service>> GetByServiceType(ServiceType serviceType)
    {
        return await _ctx.Service.Where(x => x.ServiceType == serviceType).ToListAsync();
    }

    public async Task<List<Entity.Service.Service>> Get(string name)
    {
        return await _ctx.Service.Where(x => x.Name == name).ToListAsync();
    }

    public async Task<Entity.Service.Service?> Get(Guid guid)
    {
        return await _ctx.Service.FirstOrDefaultAsync(x => x.ServiceId == guid);
    }

    public async Task<Entity.Service.Service> Insert(Entity.Service.Service service)
    {
        _ctx.Service.Add(service);
        await _ctx.SaveChangesAsync();
        return service;
    }

    public async Task<Entity.Service.Service> Update(Entity.Service.Service service)
    {
        _ctx.Service.Update(service);
        await _ctx.SaveChangesAsync();
        return service;
    }

    public async Task Delete(Entity.Service.Service service)
    {
        _ctx.Service.Remove(service);
        await _ctx.SaveChangesAsync();
    }
}