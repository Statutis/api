using System.ComponentModel.DataAnnotations;

namespace Statutis.Entity.History;

public class HistoryEntry
{
	[Required]
	public Guid ServiceId { get; set; }

	[Required]
	public Service.Service Service { get; set; }

	[Required]
	public DateTime DateTime { get; set; }

	[Required]
	public HistoryState State { get; set; } = HistoryState.Unknown;

	public String? message { get; set; } = null;

	public HistoryEntry(Service.Service service) : this(service, DateTime.Now)
	{

	}

	public HistoryEntry(Service.Service service, DateTime dateTime)
	{
		this.Service = service;
		this.DateTime = dateTime;
	}

	public HistoryEntry(Service.Service service, DateTime dateTime, HistoryState state) : this(service, dateTime)
	{
		this.State = state;
	}

	public HistoryEntry(Service.Service service, DateTime dateTime, HistoryState state, String? message) : this(service, dateTime, state)
	{
		this.message = message;
	}

	public HistoryEntry()
	{
		
	}

}