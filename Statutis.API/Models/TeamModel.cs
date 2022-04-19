using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Statutis.Entity;

namespace Statutis.API.Models;

public class TeamModel
{

	public TeamModel(Team team, IUrlHelper urlHelper)
	{
		this.Name = team.Name;
		this.Color = team.Color;
		this.Ref = urlHelper.Action("GetGuid", "Team", new { guid = team.TeamId }) ?? "";
		if (team.MainTeamId != null)
			this.MainTeamRef = urlHelper.Action("GetGuid", "Team", new { guid = team.MainTeamId }) ?? "";
	}


	public string Ref { get; set; }

	[StringLength(maximumLength: 30)]
	public String Name { get; set; }

	[StringLength(maximumLength: 10)]
	public String? Color { get; set; } = null;

	public string? MainTeamRef { get; set; } = null;
}