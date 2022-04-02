using System.ComponentModel.DataAnnotations;

namespace Statutis.Entity.Service.Check;

public class HttpService : Service
{
	public int Port { get; set; }

	public int? Code { get; set; } = null;
	
	[StringLength(maximumLength: 64)]
	public String? RedirectUrl { get; set; } = null;

	public override string GetCheckType()
	{
		return "Requête Http";
	}
}