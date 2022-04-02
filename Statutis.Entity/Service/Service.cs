using System.ComponentModel.DataAnnotations;

namespace Statutis.Entity.Service;

public abstract class Service
{
	[StringLength(maximumLength: 30), Required]
	public String Name { get; set; }

	public Guid GroupId { get; internal set; }

	public Group Group { get; set; }

	public String Description { get; set; }

	[StringLength(maximumLength: 128), Required]
	public String Host { get; set; }

	public ServiceType ServiceType { get; set; }

	public abstract String GetCheckType();

}