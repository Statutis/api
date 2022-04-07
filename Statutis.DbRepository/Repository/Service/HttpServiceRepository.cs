using Microsoft.EntityFrameworkCore;
using Statutis.Core.Interfaces.DbRepository.Service;
using Statutis.Entity.Service.Check;

namespace Statutis.DbRepository.Repository.Service;

public class HttpServiceRepository : IHttpServiceRepository
{
    private readonly StatutisContext _ctx;

    public HttpServiceRepository(StatutisContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<List<HttpService>> SelectAll()
    {
        return await _ctx.HttpService.ToListAsync();
    }

    public async Task<HttpService?> Select(Guid serviceId)
    {
        return await _ctx.HttpService.FirstOrDefaultAsync(x => x.ServiceId == serviceId);
    }

    public async Task<List<HttpService>> Select(string name)
    {
        return await _ctx.HttpService.Where(x => x.Name == name).ToListAsync();
    }

    public async Task<List<HttpService>> SelectByHostname(string hostname)
    {
        return await _ctx.HttpService.Where(x => x.Host == hostname).ToListAsync();
    }

    public async Task<HttpService> Update(HttpService httpService)
    {
        _ctx.HttpService.Update(httpService);
        await _ctx.SaveChangesAsync();
        return httpService;
    }

    public async Task<HttpService> Insert(HttpService httpService)
    {
        _ctx.HttpService.Add(httpService);
        await _ctx.SaveChangesAsync();
        return httpService;
    }

    public async Task Delete(HttpService httpService)
    {
        _ctx.HttpService.Remove(httpService);
        await _ctx.SaveChangesAsync();
    }
}