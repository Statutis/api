namespace Statutis.Entity.Service.Check;

public class PingService : Service
{

	public override string GetCheckType()
	{
		return "ping";
	}
}