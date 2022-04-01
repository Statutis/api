using System.ComponentModel.DataAnnotations;

namespace Statutis.Entity.Service;

public abstract class Service
{
	protected Service(string name, string description, Group @group, string host, ServiceType serviceType)
	{
		Name = name;
		Description = description;
		Group = @group;
		Host = host;
		ServiceType = serviceType;
	}

	[StringLength(maximumLength: 30), Required]
	public String Name { get; set; }

	public String Description { get; set; }
	
	public Group Group { get; set; }

	[StringLength(maximumLength: 128), Required]
	public String Host { get; set; }

	public ServiceType ServiceType { get; set; }

	public abstract String GetCheckType();

}