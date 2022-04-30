using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Statutis.Core.Interfaces.Business.Service;
using Statutis.Core.Interfaces.DbRepository.History;
using Statutis.Entity.History;
using Statutis.Entity.Service;
using Statutis.Entity.Service.Check;

namespace Statutis.DbRepository.Repository.History;

public class HistoryEntryRepository : IHistoryEntryRepository
{
	private readonly StatutisContext _ctx;
	private readonly IServiceService _serviceService;

	public HistoryEntryRepository(StatutisContext ctx, IServiceService serviceService)
	{
		_ctx = ctx;
		_serviceService = serviceService;
	}

	public Task<HistoryEntry?> GetLast(Entity.Service.Service service)
	{
		return _ctx.History.OrderByDescending(x => x.DateTime).FirstOrDefaultAsync(x => x.ServiceId == service.ServiceId);
	}

	public Task<HistoryEntry?> GetLast(Entity.Service.Service service, HistoryState state)
	{
		return _ctx.History.OrderByDescending(x => x.DateTime).FirstOrDefaultAsync(x => x.ServiceId == service.ServiceId && x.State == state);
	}

	public async Task<Dictionary<Entity.Service.Service, HistoryEntry?>> GetAllLast()
	{
		return await GetAllLast(await _serviceService.GetAll());


	}

	public Task<List<HistoryEntry>> Get(Entity.Service.Service service, int count = 100, ListSortDirection order = ListSortDirection.Descending)
	{
		var ctx = _ctx.History;
		if(order == ListSortDirection.Descending)
			return ctx.OrderByDescending(x => x.DateTime).Where(x => x.ServiceId == service.ServiceId).Take(count).ToListAsync();
		else
			return ctx.OrderBy(x => x.DateTime).Where(x => x.ServiceId == service.ServiceId).Take(count).ToListAsync();
	}

	public async Task<HistoryEntry> Add(HistoryEntry historyEntry)
	{
		await _ctx.History.AddAsync(historyEntry);
		await _ctx.SaveChangesAsync();
		return historyEntry;
	}

	public async Task<Dictionary<Entity.Service.Service, HistoryEntry?>> GetAllLast(List<Entity.Service.Service> services)
	{
		Dictionary<Entity.Service.Service, HistoryEntry?> dictionary = new Dictionary<Entity.Service.Service, HistoryEntry?>();

		foreach (Entity.Service.Service service in services)
		{
			dictionary.Add(service, await GetLast(service));
		}
		
		return dictionary;
	}
}