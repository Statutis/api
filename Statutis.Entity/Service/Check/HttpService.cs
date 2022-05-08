using System.ComponentModel.DataAnnotations;

namespace Statutis.Entity.Service.Check;

public class HttpService : Service
{
	
	public const String CheckType = "Requête Http";

	public int? Code { get; set; } = null;

	public override string GetCheckType()
	{
		return CheckType;
	}
}