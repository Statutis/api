using System.ComponentModel.DataAnnotations;

namespace Statutis.Entity.Service.Check;

public class HttpService : Service
{
	public HttpService(string name,
		string description,
		Group @group,
		string host,
		ServiceType serviceType,
		int port,
		int? code = null,
		string? redirectUrl = null)
		: base(name, description, @group, host, serviceType)
	{
		Port = port;
		Code = code;
		RedirectUrl = redirectUrl;
	}

	public int Port { get; set; }

	public int? Code { get; set; } = null;
	
	[StringLength(maximumLength: 64)]
	public String? RedirectUrl { get; set; } = null;

	public override string GetCheckType()
	{
		return "Requête Http";
	}
}