using Microsoft.AspNetCore.Mvc;
using Statutis.Business;
using Statutis.Entity.History;
using Statutis.Entity.Service;

namespace Statutis.API.Models;

public class GroupModel
{

	public String Id { get; set; }
	public String Ref { get; set; }

	public String Name { get; set; }

	public String Description { get; set; }

	public bool IsPublic { get; set; }

	public DateTime LastCheck { get; set; }

	public List<ServiceModel> Services { get; set; }

	public List<String> TeamsRef { get; set; }

	public GroupModel(Group group, Dictionary<Service, HistoryEntry?> services, IUrlHelper urlHelper)
	{
		Id = group.GroupId.ToString();
		Ref = urlHelper.Action("Get", "Group", new { guid = group.GroupId }) ?? String.Empty;
		Name = group.Name;
		Description = group.Description;
		IsPublic = group.IsPublic;
		
		// History
		LastCheck = services.Select(x => x.Value?.DateTime).FirstOrDefault(x => x != null) ?? DateTime.Now;
		Services = services.Select(x => x.Value == null ?
			new ServiceModel(x.Key, HistoryState.Unknown, urlHelper)
			: new ServiceModel(x.Key, x.Value, urlHelper)
		).ToList();

		
		//Team
		TeamsRef = group.Teams.Select(x => urlHelper.Action("GetGuid", "Team", new { guid = x.TeamId }) ?? String.Empty).ToList();

	}
}