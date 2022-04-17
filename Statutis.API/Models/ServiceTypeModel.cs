using System.ComponentModel.DataAnnotations;
using Statutis.Entity.Service;

namespace Statutis.API.Models;

public class ServiceTypeModel
{

	public ServiceTypeModel(ServiceType serviceType, string _ref)
	{
		Name = serviceType.Name;
		Color = serviceType.Color;
		Icon = serviceType.Icon;
		Ref = _ref;
	}


	public string Ref { get; internal set; }

	[StringLength(maximumLength: 30), Required]
	public String Name { get; }

	public byte[]? Icon { get; } = null;

	[StringLength(maximumLength: 10), Required]
	public String? Color { get; } = null;
}