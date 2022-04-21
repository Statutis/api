namespace Statutis.Entity.Service.Check;

public class DnsService : Service
{
	public const String CheckType = "DNS"; 
	public String Type { get; set; }
	public String Result { get; set; }


	public override string GetCheckType()
	{
		return CheckType;
	}
}