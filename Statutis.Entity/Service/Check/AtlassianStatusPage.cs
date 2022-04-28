namespace Statutis.Entity.Service.Check;

public class AtlassianStatusPage : Service
{
	public const String CheckType = "Atlassian Status Page";

	public String JsonUrl { get; set; }
	public override string GetCheckType()
	{
		return "Atlassian Status Page";
	}
}