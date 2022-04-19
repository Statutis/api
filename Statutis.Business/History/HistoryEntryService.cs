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

	public Task<Dictionary<Entity.Service.Service, HistoryEntry?>> GetAllLast(List<Entity.Service.Service> services)
	{
		return _repository.GetAllLast(services);
	}

	public Task<List<HistoryEntry>> Get(Service service, int count = 15)
	{
		return _repository.Get(service, count);
	}

	public Task<HistoryEntry> Add(HistoryEntry historyEntry)
	{
		return _repository.Add(historyEntry);
	}

	public async Task<Tuple<HistoryState, DateTime>> GetMainState()
	{
		List<HistoryEntry> all = (await GetAllLast()).Where(x => x.Value != null).Select(x => x.Value).ToList();
		if (!all.Any())
			return new Tuple<HistoryState, DateTime>(HistoryState.Unknown, DateTime.Now);

		DateTime lastCheck = all.OrderBy(x => x.DateTime).First().DateTime;
		if (all.Any(x => x.State != HistoryState.Online))
			return new Tuple<HistoryState, DateTime>(HistoryState.Unknown, lastCheck);

		return new Tuple<HistoryState, DateTime>(HistoryState.Online, lastCheck);
	}
}