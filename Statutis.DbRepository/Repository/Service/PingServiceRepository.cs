using Microsoft.EntityFrameworkCore;
using Statutis.Core.Interfaces.DbRepository.Service;
using Statutis.Entity.Service.Check;

namespace Statutis.DbRepository.Repository.Service;

public class PingServiceRepository : IPingServiceRepository
{
    private readonly StatutisContext _ctx;

    public PingServiceRepository(StatutisContext ctx)
    {
        _ctx = ctx;
    }
    
    
    public async Task<List<PingService>> SelectAll()
    {
        return await _ctx.PingService.ToListAsync();
    }

    public async Task<PingService?> Select(Guid serviceId)
    {
        return await _ctx.PingService.FirstOrDefaultAsync(x => x.ServiceId == serviceId);
    }

    public async Task<List<PingService>> Select(string name)
    {
        return await _ctx.PingService.Where(x => x.Name == name).ToListAsync();
    }

    public async Task<List<PingService>> SelectByHostname(string hostname)
    {
        return await _ctx.PingService.Where(x => x.Host == hostname).ToListAsync();
    }

    public async Task<PingService> Update(PingService pingService)
    {
        _ctx.PingService.Update(pingService);
        await _ctx.SaveChangesAsync();
        return pingService;
    }

    public async Task<PingService> Insert(PingService pingService)
    {
        _ctx.PingService.Add(pingService);
        await _ctx.SaveChangesAsync();
        return pingService;
    }

    public async Task Delete(PingService pingService)
    {
        _ctx.PingService.Remove(pingService);
        await _ctx.SaveChangesAsync();
    }
}