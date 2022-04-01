using System.ComponentModel.DataAnnotations;

namespace Statutis.Entity.Service;

public class ServiceType
{
	public ServiceType(string name, string? color = null, byte[]? icon = null)
	{
		Name = name;
		Color = color;
		Icon = icon;
	}

	[StringLength(maximumLength: 30), Required]
	public String Name { get; set; }
	
	public byte[]? Icon { get; set; } = null;
	
	[StringLength(maximumLength: 10), Required]
	public String? Color { get; set; } = null;
}