using Statutis.Entity.History;

namespace Statutis.API.Models;

public class MainStateModel
{
	public DateTime LastUpdate { get; set; }
	public HistoryState State { get; set; }
}