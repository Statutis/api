using System.ComponentModel.DataAnnotations;

namespace Statutis.Entity.Service;

public class Group
{
	public Group(string name, string description = "")
	{
		Name = name;
		Description = description;
	}

	public Guid GroupId;

	[StringLength(maximumLength: 30), Required]
	public String Name { get; set; }

	public String Description { get; set; }

	public List<Team> Teams { get; set; } = new List<Team>();

	public List<Service> Services { get; set; } = new List<Service>();
	

	public bool IsPublic { get; set; } = true;
}