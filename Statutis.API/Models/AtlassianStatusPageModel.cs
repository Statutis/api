using Microsoft.AspNetCore.Mvc;
using Statutis.Entity.History;
using Statutis.Entity.Service.Check;

namespace Statutis.API.Models;

/// <summary>
/// Modèle pour service avec mode de vérification AtlassianStatusPage
/// </summary>
public class AtlassianStatusPageModel : ServiceModel
{

	/// <summary>
	/// Constructeur
	/// </summary>
	/// <param name="service"></param>
	/// <param name="historyState"></param>
	/// <param name="urlHelper"></param>
	public AtlassianStatusPageModel(AtlassianStatusPageService service, HistoryState historyState, IUrlHelper urlHelper) : base(service, historyState, urlHelper)
	{
	}
}