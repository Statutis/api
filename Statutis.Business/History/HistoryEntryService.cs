using System.ComponentModel;
using Statutis.Core.Interfaces.Business.History;
using Statutis.Core.Interfaces.DbRepository.History;
using Statutis.Entity.History;
using Statutis.Entity.Service;

namespace Statutis.Business.History;

public class HistoryEntryService : IHistoryEntryService
{
	private IHistoryEntryRepository _repository;

	public HistoryEntryService(IHistoryEntryRepository repository)
	{
		_repository = repository;
	}

	public Task<HistoryEntry?> GetLast(Service service)
	{
		return _repository.GetLast(service);
	}

	public Task<HistoryEntry?> GetLast(Service service, HistoryState state)
	{
		return _repository.GetLast(service, state);
	}

	public Task<Dictionary<Service, HistoryEntry?>> GetAllLast()
	{
		return _repository.GetAllLast();
	}

	public Task<Dictionary<Service, HistoryEntry?>> GetAllLast(List<Service> services)
	{
		return _repository.GetAllLast(services);
	}

	public Task<List<HistoryEntry>> Get(Service service, int count = 100, ListSortDirection order = ListSortDirection.Descending)
	{
		return _repository.Get(service, count, order);
	}

	public Task<HistoryEntry> Add(HistoryEntry historyEntry)
	{
		return _repository.Add(historyEntry);
	}

	public async Task<Tuple<HistoryState, DateTime>> GetMainState()
	{
		List<HistoryEntry> all = (await GetAllLast()).Where(x => x.Value != null).Select(x => x.Value).ToList()!;
		if (!all.Any())
			return new Tuple<HistoryState, DateTime>(HistoryState.Unknown, DateTime.Now);

		DateTime lastCheck = all.OrderBy(x => x.DateTime).First().DateTime;
		if (all.Any(x => x.State != HistoryState.Online))
		{
			if (all.Any(x => x.State == HistoryState.Error))
				return new Tuple<HistoryState, DateTime>(HistoryState.Error, lastCheck);
			
			if (all.Any(x => x.State == HistoryState.Unreachable))
				return new Tuple<HistoryState, DateTime>(HistoryState.Unreachable, lastCheck);
			
			return new Tuple<HistoryState, DateTime>(HistoryState.Unknown, lastCheck);
		}

		return new Tuple<HistoryState, DateTime>(HistoryState.Online, lastCheck);
	}

	public async Task<List<HistoryEntry>> GetHistoryEntryFromAGroup(Group group, ListSortDirection order = ListSortDirection.Descending)
	{
		List<HistoryEntry> res = new List<HistoryEntry>();
		
		if (group.Services.Count == 0)
			return res;

		foreach (Service service in group.Services)
		{
			var historyEntry = await this.Get(service);
			historyEntry.ForEach(x => { x.Service = null!;});
			if(historyEntry.Count == 0)
				continue;
			
			res.AddRange(historyEntry);
		}

		if (order == ListSortDirection.Ascending)
		{
			res = res.OrderBy(x => x.DateTime).ToList();
		}
		else
		{
			res = res.OrderByDescending(x => x.DateTime).ToList();
		}

		return res;
	}
}