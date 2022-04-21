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

	public List<ServiceModel> Services { get; set; }

	public List<String> TeamsRef { get; set; }

	public GroupModel(Group group, Dictionary<Service, HistoryEntry?> services, IUrlHelper urlHelper)
	{
		Ref = urlHelper.Action("Get", "Group", new { guid = group.GroupId }) ?? String.Empty;
		if (group.MainGroupId != null)
			MainGroupRef = Ref = urlHelper.Action("Get", "Group", new { guid = group.MainGroupId }) ?? String.Empty;
		Name = group.Name;
		Description = group.Description;
		LastCheck = services.Select(x => x.Value?.DateTime).FirstOrDefault(x => x != null) ?? DateTime.Now;
		Services = services.Select(x => x.Value == null ?
			new ServiceModel(x.Key, HistoryState.Unknown, urlHelper)
			: new ServiceModel(x.Key, x.Value, urlHelper)
		).ToList();

		TeamsRef = group.Teams.Select(x => urlHelper.Action("GetGuid","Team", new {guid = x.TeamId}) ?? String.Empty).ToList();

	}
}