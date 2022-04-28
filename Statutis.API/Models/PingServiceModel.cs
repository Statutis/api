using Microsoft.AspNetCore.Mvc;
using Statutis.Entity.History;
using Statutis.Entity.Service;

namespace Statutis.API.Models;

public class PingServiceModel : ServiceModel
{
    public PingServiceModel(Service service, HistoryState historyState, IUrlHelper urlHelper) : base(service, historyState, urlHelper)
    {
    }

    public PingServiceModel(Service service, HistoryEntry entry, IUrlHelper urlHelper) : base(service, entry, urlHelper)
    {
    }
}