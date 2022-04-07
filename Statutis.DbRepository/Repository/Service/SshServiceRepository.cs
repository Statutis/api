using Microsoft.EntityFrameworkCore;
using Statutis.Core.Interfaces.DbRepository.Service;
using Statutis.Entity.Service.Check;

namespace Statutis.DbRepository.Repository.Service;

public class SshServiceRepository : ISshServiceRepository
{
    private readonly StatutisContext _ctx;

    public SshServiceRepository(StatutisContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<List<SshService>> SelectAll()
    {
        return await _ctx.SshService.ToListAsync();
    }

    public async Task<SshService?> Select(Guid serviceId)
    {
        return await _ctx.SshService.FirstOrDefaultAsync(x => x.ServiceId == serviceId);
    }

    public async Task<List<SshService>> Select(string name)
    {
        return await _ctx.SshService.Where(x => x.Name == name).ToListAsync();
    }

    public async Task<List<SshService>> SelectByHostname(string hostname)
    {
        return await _ctx.SshService.Where(x => x.Host == hostname).ToListAsync();
    }

    public async Task<SshService> Update(SshService sshService)
    {
        _ctx.SshService.Update(sshService);
        await _ctx.SaveChangesAsync();
        return sshService;
    }

    public async Task<SshService> Insert(SshService sshService)
    {
        _ctx.SshService.Add(sshService);
        await _ctx.SaveChangesAsync();
        return sshService;
    }

    public async Task Delete(SshService sshService)
    {
        _ctx.SshService.Remove(sshService);
        await _ctx.SaveChangesAsync();
    }
}