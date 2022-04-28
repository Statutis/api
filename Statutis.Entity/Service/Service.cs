using System.ComponentModel.DataAnnotations;
using Statutis.Entity.History;

namespace Statutis.Entity.Service;

public abstract class Service
{

	public Guid ServiceId { get; set; }

	[StringLength(maximumLength: 30), Required]
	public String Name { get; set; } = String.Empty;

	public Guid GroupId { get; set; }

	public Group Group { get; set; } = null!;

	public String Description { get; set; } = String.Empty;

	[StringLength(maximumLength: 128), Required]
	public String Host { get; set; } = String.Empty;

	public String ServiceTypeName { get; set; } = String.Empty;
	public ServiceType ServiceType { get; set; } = null!;

	public abstract String GetCheckType();

	public List<HistoryEntry> HistoryEntries { get; set; } = new();

}