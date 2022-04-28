using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Statutis.Entity;

namespace Statutis.API.Models;

/// <summary>
/// Modèle des équipes
/// </summary>
public class TeamModel
{

	/// <summary>
	/// Constructeur
	/// </summary>
	/// <param name="team"></param>
	/// <param name="urlHelper"></param>
	public TeamModel(Team team, IUrlHelper urlHelper)
	{
		this.Id = team.TeamId.ToString();
		this.Name = team.Name;
		this.Ref = urlHelper.Action("GetGuid", "Team", new { guid = team.TeamId }) ?? "";
		this.AvatarRef = team.Avatar != null && team.AvatarContentType != null ? urlHelper.Action("GetAvatar", "Team", new { guid = team.TeamId }) ?? null : null;

		this.UserRef = team.Users.Select(x => urlHelper.Action("GetByEmail", "User", new { email = x.Email }) ?? String.Empty).ToList();
		this.GroupRef = team.Groups.Select(x => urlHelper.Action("Get", "Group", new { guid = x.GroupId }) ?? String.Empty).ToList();
	}


	/// <summary>
	/// Identifiant unique de l'équipe
	/// </summary>
	public string Id { get; }
	
	/// <summary>
	/// Référence de l'équipe
	/// </summary>
	public string Ref { get; }

	/// <summary>
	/// Nom de l'équipe
	/// </summary>
	[StringLength(maximumLength: 30)]
	public String Name { get; }

	/// <summary>
	/// Références vers les utilisateurs de cette équipe 
	/// </summary>
	public List<string> UserRef { get; }

	
	/// <summary>
	/// Références vers les groupes de cette équipe 
	/// </summary>
	public List<string> GroupRef { get; }
	
	
	/// <summary>
	/// Références vers l'avatar de cette équipe 
	/// </summary>
	public String? AvatarRef { get; }
}