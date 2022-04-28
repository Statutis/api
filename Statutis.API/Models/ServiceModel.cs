using Microsoft.AspNetCore.Mvc;
using Statutis.Entity.History;
using Statutis.Entity.Service;

namespace Statutis.API.Models;

/// <summary>
/// Modlèle pour les services
/// </summary>
public class ServiceModel
{
	
	/// <summary>
	/// Référence ver le service
	/// </summary>
	public String Ref { get; set; }

	
	/// <summary>
	/// Référence vers le type de service
	/// </summary>
	public String ServiceTypeRef { get; set; }

	
	/// <summary>
	/// Mode de vérification
	/// </summary>
	public String CheckType { get; set; }

	
	/// <summary>
	/// Nom du service
	/// </summary>
	public String Name { get; set; }

	
	/// <summary>
	/// Description du service
	/// </summary>
	public String Description { get; set; }

	/// <summary>
	/// Hôte ciblé par ce service
	/// </summary>
	public String Host { get; set; }

	/// <summary>
	/// Dernier état du service
	/// </summary>
	public HistoryState State { get; set; }

	/// <summary>
	/// Date de dernière vérification de l'état du service
	/// </summary>
	public DateTime LastCheck { get; set; }
	
	/// <summary>
	/// Référence vers la dernière dans l'historique concernant ce service
	/// </summary>
	public string HistoryRef { get; set; }

	/// <summary>
	/// Référence vers le groupe de ce service
	/// </summary>
	public String GroupRef { get; set; }


	/// <summary>
	/// Constructeur
	/// </summary>
	/// <param name="service"></param>
	/// <param name="historyState"></param>
	/// <param name="urlHelper"></param>
	public ServiceModel(Service service, HistoryState historyState, IUrlHelper urlHelper)
	{
		Ref = urlHelper.Action("Get", "Service", new { guid = service.ServiceId }) ?? String.Empty;
		CheckType = service.GetCheckType();
		Name = service.Name;
		Description = service.Description;
		Host = service.Host;

		// Ref
		GroupRef = urlHelper.Action("Get", "Group", new { guid = service.GroupId }) ?? String.Empty;
		ServiceTypeRef = urlHelper.Action("Get", "ServiceType", new { Name = service.ServiceTypeName }) ?? String.Empty;
		
		// History
		State = historyState;
		LastCheck = DateTime.Now;
		HistoryRef = urlHelper.Action("Get","History", new {Guid = service.ServiceId}) ?? String.Empty;
	}

	public ServiceModel(Service service, HistoryEntry entry, IUrlHelper urlHelper) : this(service, entry.State, urlHelper)
	{
		LastCheck = entry.DateTime;
	}

}