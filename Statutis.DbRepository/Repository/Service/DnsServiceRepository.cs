using Microsoft.EntityFrameworkCore;
using Statutis.Core.Interfaces.DbRepository.Service;
using Statutis.Entity.Service.Check;

namespace Statutis.DbRepository.Repository.Service;

public class DnsServiceRepository : IDnsServiceRepository
{
    private readonly StatutisContext _ctx;

    public DnsServiceRepository(StatutisContext ctx)
    {
        _ctx = ctx;
    }


    public async Task<List<DnsService>> SelectAll()
    {
        return await _ctx.DnsService.ToListAsync();
    }

    public async Task<DnsService?> Select(Guid serviceId)
    {
        return await _ctx.DnsService.FirstOrDefaultAsync(x => x.ServiceId == serviceId);
    }

    public async Task<List<DnsService>> Select(string name)
    {
        return await _ctx.DnsService.Where(x => x.Name == name).ToListAsync();
    }

    public async Task<List<DnsService>> SelectByHostname(string hostname)
    {
        return await _ctx.DnsService.Where(x => x.Host == hostname).ToListAsync();
    }

    public async Task<DnsService> Update(DnsService dnsService)
    {
        _ctx.DnsService.Update(dnsService);
        await _ctx.SaveChangesAsync();
        return dnsService;
    }

    public async Task<DnsService> Insert(DnsService dnsService)
    {
        _ctx.DnsService.Add(dnsService);
        await _ctx.SaveChangesAsync();
        return dnsService;
    }

    public async Task Delete(DnsService dnsService)
    {
        _ctx.DnsService.Remove(dnsService);
        await _ctx.SaveChangesAsync();
    }
}