using Microsoft.AspNetCore.Mvc;
using Statutis.Entity.History;
using Statutis.Entity.Service;

namespace Statutis.API.Models;

/// <summary>
/// Formulaire pour les services en mode de vérification Ping
/// </summary>
public class PingServiceModel : ServiceModel
{
	/// <summary>
	/// Constructeur
	/// </summary>
	/// <param name="service"></param>
	/// <param name="historyState"></param>
	/// <param name="urlHelper"></param>
	public PingServiceModel(Service service, HistoryState historyState, IUrlHelper urlHelper) : base(service, historyState, urlHelper)
	{
	}

	/// <summary>
	/// Constructeur
	/// </summary>
	/// <param name="service"></param>
	/// <param name="entry"></param>
	/// <param name="urlHelper"></param>
	public PingServiceModel(Service service, HistoryEntry entry, IUrlHelper urlHelper) : base(service, entry, urlHelper)
	{
	}
}