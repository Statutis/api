namespace Statutis.Entity.Service.Check;

public class PingService : Service
{
	
	public const String CheckType = "Ping";
	
	public override string GetCheckType()
	{
		return CheckType;
	}
}