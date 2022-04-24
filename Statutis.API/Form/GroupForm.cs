using System.ComponentModel.DataAnnotations;

namespace Statutis.API.Form;

public class GroupForm
{
	[StringLength(maximumLength: 30), Required]
	public String Name { get; set; }

	public String Description { get; set; }

	public List<String> Teams { get; set; } = new List<String>();


	public bool IsPublic { get; set; } = true;
}