namespace Statutis.Entity.Service.Check;

public class DnsService : Service
{

	public String Type { get; set; }
	public String Result { get; set; }

	public DnsService(
		string name,
		string description,
		Group @group,
		string host,
		ServiceType serviceType,
		string type,
		string result) : base(name, description, @group, host, serviceType)
	{
		Type = type;
		Result = result;
	}

	public override string GetCheckType()
	{
		return "DNS";
	}
}