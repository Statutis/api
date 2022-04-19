using System.ComponentModel.DataAnnotations;
using Statutis.Entity.History;

namespace Statutis.Entity.Service;

public abstract class Service
{

	public Guid ServiceId { get; set; }

	[StringLength(maximumLength: 30), Required]
	public String Name { get; set; }

	public Guid GroupId { get; set; }

	public Group Group { get; set; }

	public String Description { get; set; }

	[StringLength(maximumLength: 128), Required]
	public String Host { get; set; }

	public String ServiceTypeName { get; set; }
	public ServiceType ServiceType { get; set; }

	public bool IsPublic { get; set; } = true;

	public abstract String GetCheckType();

	public List<HistoryEntry> HistoryEntries { get; set; }

}