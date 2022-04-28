using Microsoft.AspNetCore.Mvc;
using Statutis.Entity.History;
using Statutis.Entity.Service.Check;

namespace Statutis.API.Models;

/// <summary>
/// Modèle de service DNS
/// </summary>
public class DnsServiceModel : ServiceModel
{

	/// <summary>
	/// Type de requête DNS
	/// </summary>
	public string Type { get; set; }

	/// <summary>
	/// Résultat de la requête
	/// </summary>
	public string Result { get; set; }


	/// <summary>
	/// Constructeur
	/// </summary>
	/// <param name="dnsService"></param>
	/// <param name="historyState"></param>
	/// <param name="urlHelper"></param>
	public DnsServiceModel(DnsService dnsService, HistoryState historyState, IUrlHelper urlHelper) : base(dnsService, historyState, urlHelper)
	{
		this.Type = dnsService.Type;
		this.Result = dnsService.Result;
	}
}