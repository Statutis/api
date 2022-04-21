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
	}

	[StringLength(maximumLength: 50), Required]
	public String Email { get; set; }

	[StringLength(maximumLength: 30), Required]
	public String Username { get; set; }

	[StringLength(maximumLength: 255), Required, JsonIgnore]
	public String Password { get; set; }

	public byte[]? Avatar { get; set; } = null;

	public String Roles { get; set; }

	public List<Team> Teams { get; set; } = new List<Team>();




}