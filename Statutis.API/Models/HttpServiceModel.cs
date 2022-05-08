using Microsoft.AspNetCore.Mvc;
using Statutis.Entity.History;
using Statutis.Entity.Service;
using Statutis.Entity.Service.Check;

namespace Statutis.API.Models;

/// <summary>
/// Modèle pour les services en mode de vérification HTTP
/// </summary>
public class HttpServiceModel : ServiceModel
{

    /// <summary>
    /// Code de retour attendu
    /// </summary>
    public int? Code { get; set; } = null;
    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="service"></param>
    /// <param name="entry"></param>
    /// <param name="urlHelper"></param>
    public HttpServiceModel(HttpService service, HistoryEntry entry, IUrlHelper urlHelper) : this(service, entry.State, urlHelper)
    {
    }

    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="service"></param>
    /// <param name="historyState"></param>
    /// <param name="urlHelper"></param>
    public HttpServiceModel(HttpService service, HistoryState historyState, IUrlHelper urlHelper) : base(service,
        historyState, urlHelper)
    {
        Code = service.Code;
    }
}