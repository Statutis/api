namespace Statutis.Entity.Service.Check;

public class DnsService : Service
{

	public String Type { get; set; }
	public String Result { get; set; }


	public override string GetCheckType()
	{
		return "DNS";
	}
}