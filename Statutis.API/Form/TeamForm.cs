using System.ComponentModel.DataAnnotations;

namespace Statutis.API.Form;

public class TeamForm
{
	public String Name { get; set; }

	[StringLength(maximumLength: 10)]
	public String? Color { get; set; } = null;
	
	public List<String> Users { get; set; } = new List<String>();
}