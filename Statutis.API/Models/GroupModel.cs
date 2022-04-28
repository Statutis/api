using Microsoft.AspNetCore.Mvc;
using Statutis.Entity.History;
using Statutis.Entity.Service;

namespace Statutis.API.Models;

/// <summary>
/// Modèle de groupe
/// </summary>
public class GroupModel
{

	/// <summary>
	/// Identifiant du groupe
	/// </summary>
	public String Id { get; set; }
	
	/// <summary>
	/// Référence vers le groupe cible
	/// </summary>
	public String Ref { get; set; }

	/// <summary>
	/// Nom du groupe
	/// </summary>
	public String Name { get; set; }

	/// <summary>
	/// Description du groupe
	/// </summary>
	public String Description { get; set; }
	
	/// <summary>
	/// Visibilité du groupe 
	/// </summary>
	public bool IsPublic { get; set; }

	/// <summary>
	/// Dernière vérification de l'état du groupe
	/// </summary>
	public DateTime LastCheck { get; set; }

	/// <summary>
	/// Liste des services du groupe
	/// </summary>
	public List<ServiceModel> Services { get; set; }

	/// <summary>
	/// Références vers les équipes de ce groupe
	/// </summary>
	public List<String> TeamsRef { get; set; }

	/// <summary>
	/// Référence vers l'avatar du groupe
	/// </summary>
	public String? AvatarRef { get; set; }

	/// <summary>
	/// Constructeur
	/// </summary>
	/// <param name="group"></param>
	/// <param name="services"></param>
	/// <param name="urlHelper"></param>
	public GroupModel(Group group, Dictionary<Service, HistoryEntry?> services, IUrlHelper urlHelper)
	{
		Id = group.GroupId.ToString();
		Ref = urlHelper.Action("Get", "Group", new { guid = group.GroupId }) ?? String.Empty;
		AvatarRef = group.Avatar != null && group.AvatarContentType != null ? urlHelper.Action("GetAvatar", "Group", new { guid = group.GroupId }) ?? null : null;
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