using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Statutis.Entity;

namespace Statutis.API.Models;

public class TeamModel
{

	public TeamModel(Team team, IUrlHelper urlHelper)
	{
		this.Id = team.TeamId.ToString();
		this.Name = team.Name;
		this.Color = team.Color;
		this.Ref = urlHelper.Action("GetGuid", "Team", new { guid = team.TeamId }) ?? "";
		this.AvatarRef = team.Avatar != null && team.AvatarContentType != null ? urlHelper.Action("GetAvatar", "Team", new { guid = team.TeamId }) ?? null : null;

		this.UserRef = team.Users.Select(x => urlHelper.Action("GetByEmail", "User", new { email = x.Email }) ?? String.Empty).ToList();
		this.GroupRef = team.Groups.Select(x => urlHelper.Action("Get", "Group", new { guid = x.GroupId }) ?? String.Empty).ToList();
	}


	public string Id { get; set; }
	public string Ref { get; set; }

	[StringLength(maximumLength: 30)]
	public String Name { get; set; }

	[StringLength(maximumLength: 10)]
	public String? Color { get; set; } = null;

	public List<string> UserRef { get; set; }

	public List<string> GroupRef { get; set; }
	
	public String? AvatarRef { get; set; }
}