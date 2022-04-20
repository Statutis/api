using Microsoft.AspNetCore.Mvc;
using Statutis.Business;
using Statutis.Entity.History;
using Statutis.Entity.Service;

namespace Statutis.API.Models;

public class GroupModel
{

	public String Ref { get; set; }

	public String? MainGroupRef { get; set; } = null;

	public String Name { get; set; }

	public String Description { get; set; }

	public DateTime LastCheck { get; set; }

	public List<GroupServiceModel> Services { get; set; }

	public GroupModel(Group group, Dictionary<Service, HistoryEntry?> services, IUrlHelper urlHelper)
	{
		Ref = urlHelper.Action("Get", "Group", new { guid = group.GroupId }) ?? String.Empty;
		if (group.MainGroupId != null)
			MainGroupRef = Ref = urlHelper.Action("Get", "Group", new { guid = group.MainGroupId }) ?? String.Empty;
		Name = group.Name;
		Description = group.Description;
		LastCheck = services.Select(x => x.Value?.DateTime).FirstOrDefault(x => x != null) ?? DateTime.Now;
		Services = services.Select(x => x.Value == null ?
			new GroupServiceModel(x.Key, HistoryState.Unknown, urlHelper)
			: new GroupServiceModel(x.Key, x.Value, urlHelper)
		).ToList();

	}
}

public class GroupServiceModel
{
	public String Ref { get; set; }

	public HistoryState state { get; set; }

	public DateTime lastCheck { get; set; }

	public GroupServiceModel(Service service, HistoryState historyState, IUrlHelper urlHelper)
	{
		Ref = urlHelper.Action("Get", "Service", new { guid = service.ServiceId }) ?? String.Empty;
		state = historyState;
		lastCheck = DateTime.Now;
	}

	public GroupServiceModel(Service service, HistoryEntry entry, IUrlHelper urlHelper)
	{
		Ref = urlHelper.Action("Get", "Service", new { guid = service.ServiceId }) ?? String.Empty;
		state = entry.State;
		lastCheck = entry.DateTime;
	}

}