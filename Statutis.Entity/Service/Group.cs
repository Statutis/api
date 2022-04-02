using System.ComponentModel.DataAnnotations;

namespace Statutis.Entity.Service;

public class Group
{
	public Group(string name, string description = "")
	{
		Name = name;
		Description = description;
	}

	public Group(string name, string description, Group mainGroup) : this(name, description)
	{

		MainGroup = mainGroup;
	}

	public Guid GroupId;

	[StringLength(maximumLength: 30), Required]
	public String Name { get; set; }

	public String Description { get; set; }

	public Guid? MainGroupId { get; internal set; } = null;
	public Group? MainGroup { get; set; } = null;

	public List<Team> Teams { get; set; } = new List<Team>();

	public List<Service> Services { get; set; } = new List<Service>();
	public List<Group> Children { get; set; } = new List<Group>();
}