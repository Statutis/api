using Microsoft.EntityFrameworkCore;
using Statutis.Core.Interfaces.Business.Service;
using Statutis.Core.Interfaces.DbRepository.History;
using Statutis.Entity.History;
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

		var req = _ctx.History.AsQueryable();

		return  (await _serviceService.GetAll())
			.Select(async x => await _ctx.History
				.OrderByDescending(y => y.DateTime)
				.FirstOrDefaultAsync(y => y.ServiceId == x.ServiceId)
			)
			.Select(x=>x.Result)
			.Where(x=>x != null).ToDictionary(x => x.Service, x => x);
		

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