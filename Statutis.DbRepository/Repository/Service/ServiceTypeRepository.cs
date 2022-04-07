using Microsoft.EntityFrameworkCore;
using Statutis.Core.Interfaces.DbRepository.Service;
using Statutis.Entity.Service;

namespace Statutis.DbRepository.Repository.Service;

public class ServiceTypeRepository : IServiceTypeRepository

{
    private readonly StatutisContext _ctx;

    public ServiceTypeRepository(StatutisContext ctx)
    {
        _ctx = ctx;
    }


    public async Task<List<ServiceType>> GetAll()
    {
        return await _ctx.ServiceType.ToListAsync();
    }

    public async Task<ServiceType?> Get(string name)
    {
        return await _ctx.ServiceType.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<ServiceType> Insert(ServiceType serviceType)
    {
        _ctx.ServiceType.Add(serviceType);
        await _ctx.SaveChangesAsync();
        return serviceType;
    }

    public async Task<ServiceType> Update(ServiceType serviceType)
    {
        _ctx.ServiceType.Update(serviceType);
        await _ctx.SaveChangesAsync();
        return serviceType;
    }

    public async Task Delete(ServiceType serviceType)
    {
        _ctx.ServiceType.Remove(serviceType);
        await _ctx.SaveChangesAsync();
    }
}