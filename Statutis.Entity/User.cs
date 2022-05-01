using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Statutis.Entity;

public class User
{
	public User(string email, string username, string password)
	{
		this.Email = email;
		this.Username = username;
		this.Password = password;
		this.Avatar = null;
		this.Roles = String.Empty;
	}


	[StringLength(maximumLength: 50)]
	public String? Name { get; set; }

	[StringLength(maximumLength: 50)]
	public String? Firstname { get; set; }


	[StringLength(maximumLength: 50), Required]
	public String Email { get; set; }

	[StringLength(maximumLength: 30), Required]
	public String Username { get; set; }

	[StringLength(maximumLength: 255), Required, JsonIgnore]
	public String Password { get; set; }

	public byte[]? Avatar { get; set; } = null;

	public String? AvatarContentType { get; set; } = null;

	public String Roles { get; set; }

	public List<Team> Teams { get; set; } = new List<Team>();

	public String CompleteName()
	{
		return String.IsNullOrWhiteSpace(Firstname) || String.IsNullOrWhiteSpace(Firstname) ? Username : (Firstname + Name);
	}

	public bool IsAdmin()
	{
		return this.Roles.ToUpper().Trim() == "ROLE_ADMIN";
	}
}