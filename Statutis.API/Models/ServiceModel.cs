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

	public bool IsPublic { get; set; } = true;

	public HistoryState State { get; set; }

	public DateTime LastCheck { get; set; }


	public ServiceModel(Service service, HistoryState historyState, IUrlHelper urlHelper)
	{
		Ref = urlHelper.Action("Get", "Service", new { guid = service.ServiceId }) ?? String.Empty;
		State = historyState;
		LastCheck = DateTime.Now;

		ServiceTypeRef = urlHelper.Action("Get", "ServiceType", new { Name = service.ServiceTypeName }) ?? String.Empty;
		CheckType = service.GetCheckType();
		Name = service.Name;
		Description = service.Description;
		Host = service.Host;
		IsPublic = service.IsPublic;
	}

	public ServiceModel(Service service, HistoryEntry entry, IUrlHelper urlHelper) : this(service, entry.State, urlHelper)
	{
		LastCheck = entry.DateTime;
	}

}