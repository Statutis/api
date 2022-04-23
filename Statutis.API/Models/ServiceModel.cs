using Microsoft.AspNetCore.Mvc;
using Statutis.Entity.History;
using Statutis.Entity.Service;

namespace Statutis.API.Models;

public class ServiceModel
{
	public String Ref { get; set; }

	public String ServiceTypeRef { get; set; }

	public String CheckType { get; set; }

	public String Name { get; set; }

	public String Description { get; set; }

	public String Host { get; set; }

	public HistoryState State { get; set; }

	public DateTime LastCheck { get; set; }
	
	public string HistoryRef { get; set; }

	public String GroupRef { get; set; }


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