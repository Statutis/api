using System.ComponentModel.DataAnnotations;

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

	[StringLength(maximumLength: 255), Required]
	public String Password { get; set; }

	public byte[]? Avatar { get; set; } = null;

	public List<String> Roles { get; set; } = new List<string>();

	public List<Team> Teams { get; set; } = new List<Team>();




}