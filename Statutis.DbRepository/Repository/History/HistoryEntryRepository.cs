using Microsoft.EntityFrameworkCore;
using Statutis.Core.Interfaces.DbRepository.History;
using Statutis.Entity.History;

namespace Statutis.DbRepository.Repository.History;

public class HistoryEntryRepository : IHistoryEntryRepository
{
	private readonly StatutisContext _ctx;

	public HistoryEntryRepository(StatutisContext ctx)
	{
		_ctx = ctx;
	}

	public Task<HistoryEntry?> GetLast(Entity.Service.Service service)
	{
		return _ctx.History.OrderByDescending(x => x.DateTime).FirstOrDefaultAsync(x => x.ServiceId == service.ServiceId);
	}

	public Task<HistoryEntry?> GetLast(Entity.Service.Service service, HistoryState state)
	{
		return _ctx.History.OrderByDescending(x => x.DateTime).FirstOrDefaultAsync(x => x.ServiceId == service.ServiceId && x.State == state);
	}

	public Task<Dictionary<Entity.Service.Service, HistoryEntry?>> GetAllLast()
	{
		return _ctx.History
			.OrderByDescending(x => x.DateTime)
			.GroupBy(x=>x.Service)
			.ToDictionaryAsync(x=>x.Key, x=>x.FirstOrDefault());
	}

	public Task<List<HistoryEntry>> Get(Entity.Service.Service service, int count = 15)
	{
		return _ctx.History.OrderByDescending(x => x.DateTime).Where(x => x.ServiceId == service.ServiceId).Take(count).ToListAsync();
	}

	public async Task<HistoryEntry> Add(HistoryEntry historyEntry)
	{
		await _ctx.History.AddAsync(historyEntry);
		await _ctx.SaveChangesAsync();
		return historyEntry;
	}
}