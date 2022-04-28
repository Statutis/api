using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Statutis.Entity;

namespace Statutis.API.Models;

/// <summary>
/// Modèle d'un utilisateur
/// </summary>
public class UserModel
{

	/// <summary>
	/// Référence vers un utilisateur
	/// </summary>
	public String Ref { get; set; }
	
	/// <summary>
	/// Identifiant et Email d'un utilisateur
	/// </summary>
	public String Email { get; set; }

	/// <summary>
	/// Nom d'utilisateur
	/// </summary>
	public String Username { get; set; }

	/// <summary>
	/// Référence vers l'avatar de l'utilisateur
	/// </summary>
	public String? AvatarRef { get; set; } = null;

	/// <summary>
	/// Nom de l'utilisateur
	/// </summary>
	public String? Name { get; set; }

	/// <summary>
	/// Prènom de l'utilisateur
	/// </summary>
	public String? Firstname { get; set; }
	
	/// <summary>
	/// Liste des roles de l'utilisateur
	/// </summary>
	public List<String> Roles { get; set; } = new List<string>();

	/// <summary>
	/// Références vers les équipes de l'utilisateur
	/// </summary>
	public List<String> TeamsRef { get; set; }

	/// <summary>
	/// Constructeur
	/// </summary>
	/// <param name="user"></param>
	/// <param name="url"></param>
	public UserModel(User user, IUrlHelper url)
	{
		Ref = url.Action("GetByEmail", "User", new { email = user.Email }) ?? String.Empty;
		Email = user.Email;
		Username = user.Username;
		Name = user.Name;
		Firstname = user.Firstname;

		if (user.Avatar != null && user.AvatarContentType != null)
			AvatarRef = url.Action("GetAvatar", "User", new { email = Email });

		Roles.Add(user.Roles);

		TeamsRef = user.Teams.Select(x => url.Action("GetGuid", "Team", new { guid = x.TeamId }) ?? String.Empty).ToList();

	}

}