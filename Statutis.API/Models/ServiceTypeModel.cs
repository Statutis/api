using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Statutis.Entity.Service;

namespace Statutis.API.Models;

/// <summary>
/// Modèle des types de services
/// </summary>
public class ServiceTypeModel
{

	/// <summary>
	/// Constructeur
	/// </summary>
	/// <param name="serviceType"></param>
	/// <param name="urlHelper"></param>
	public ServiceTypeModel(ServiceType serviceType, IUrlHelper urlHelper)
	{
		Name = serviceType.Name;
		Ref = urlHelper.Action("Get", "ServiceType", new { name = serviceType.Name }) ?? "";
	}


	/// <summary>
	/// Référence vers le type de service
	/// </summary>
	public string Ref { get; }

	/// <summary>
	/// Nom du type de service
	/// </summary>
	[StringLength(maximumLength: 30), Required]
	public String Name { get; }
}