namespace Statutis.Entity.Service.Check;

public class PingService : Service
{

	public PingService(string name, string description, Group @group, string host, ServiceType serviceType) : base(name, description, @group, host, serviceType)
	{
	}

	public override string GetCheckType()
	{
		return "ping";
	}
}