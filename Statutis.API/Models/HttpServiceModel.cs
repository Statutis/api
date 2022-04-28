using Microsoft.AspNetCore.Mvc;
using Statutis.Entity.History;
using Statutis.Entity.Service;
using Statutis.Entity.Service.Check;

namespace Statutis.API.Models;

public class HttpServiceModel : ServiceModel
{
    public int Port { get; set; }

    public int? Code { get; set; } = null;

    public String? RedirectUrl { get; set; } = null;

    public HttpServiceModel(HttpService service, HistoryEntry entry, IUrlHelper urlHelper) : this(service, entry.State, urlHelper)
    {
    }

    public HttpServiceModel(HttpService service, HistoryState historyState, IUrlHelper urlHelper) : base(service,
        historyState, urlHelper)
    {
        Port = service.Port;
        Code = service.Code;
        RedirectUrl = service.RedirectUrl;
    }
}