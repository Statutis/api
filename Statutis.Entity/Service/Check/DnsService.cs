namespace Statutis.Entity.Service.Check;

public class DnsService : Service
{
	public const String CheckType = "DNS";
	public String Type { get; set; } = String.Empty;
	public String Result { get; set; } = String.Empty;


	public override string GetCheckType()
	{
		return CheckType;
	}
}