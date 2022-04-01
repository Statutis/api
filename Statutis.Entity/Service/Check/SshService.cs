using System.ComponentModel.DataAnnotations;

namespace Statutis.Entity.Service.Check;

public class SshService : Service
{
	public SshService(string name,
		string description,
		Group @group,
		string host,
		ServiceType serviceType,
		string bash,
		string username,
		string password,
		bool isSshKey = false,
		int port = 22) : base(name, description, @group, host, serviceType)
	{
		Port = port;
		Bash = bash;
		Username = username;
		Password = password;
		IsSshKey = isSshKey;
	}

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