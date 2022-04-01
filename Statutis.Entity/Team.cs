using System.ComponentModel.DataAnnotations;
using Statutis.Entity.Service;

namespace Statutis.Entity;

public class Team
{
	public Team(string name, string? color = null, Team? mainTeam = null)
	{
		Name = name;
		MainTeam = mainTeam;
		Color = color;
	}

	[StringLength(maximumLength: 30), Required]
	public String Name { get; set; }

	[StringLength(maximumLength: 10)]
	public String? Color { get; set; } = null;

	public Team? MainTeam { get; set; } = null;

	public List<User> Users { get; set; } = new List<User>();
	
	public List<Group> Groups { get; set; } = new List<Group>();

}