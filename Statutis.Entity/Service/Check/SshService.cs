using System.ComponentModel.DataAnnotations;

namespace Statutis.Entity.Service.Check;

public class SshService : Service
{

	public int Port { get; set; }

	[StringLength(maximumLength: 255), Required]
	public String Bash { get; set; }

	[StringLength(maximumLength: 64), Required]
	public String Username { get; set; }

	[Required]
	public String Password { get; set; }

	public bool IsSshKey { get; set; }

	public override string GetCheckType()
	{
		return "SSH";
	}
}