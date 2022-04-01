using System.ComponentModel.DataAnnotations;

namespace Statutis.Entity.Service;

public class Group
{
	public Group(string name, string description = "", Group? mainGroup = null)
	{
		Name = name;
		Description = description;
		MainGroup = mainGroup;
	}

	[StringLength(maximumLength: 30), Required]
	public String Name { get; set; }

	public String Description { get; set; }

	public Group? MainGroup { get; set; } = null;

	public List<Team> Teams { get; set; } = new List<Team>();
}