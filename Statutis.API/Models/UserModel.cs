using Microsoft.AspNetCore.Mvc;
using Statutis.Entity;

namespace Statutis.API.Models;

public class UserModel
{

	public String Email { get; set; }

	public String Username { get; set; }

	public String? AvatarRef { get; set; } = null;

	public List<String> Roles { get; set; } = new List<string>();

	public List<String> TeamsRef { get; set; } = new List<String>();

	public UserModel(User user, IUrlHelper url)
	{
		Email = user.Email;
		Username = user.Username;
		AvatarRef = url.Action("GetAvatar","User",new {email = Email});
		Roles.Add(user.Roles);

		TeamsRef = user.Teams.Select(x => url.Action("GetGuid", "Team", new { guid = x.TeamId }) ?? String.Empty).ToList();

	}

}