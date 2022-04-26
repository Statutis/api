using Microsoft.AspNetCore.Mvc;
using Statutis.Entity.History;
using Statutis.Entity.Service.Check;

namespace Statutis.API.Models;

public class DnsServiceModel : ServiceModel
{
    public string Type { get; set; }
    public string Result { get; set; }

    public DnsServiceModel(DnsService dnsService, HistoryState historyState, IUrlHelper urlHelper) : base(dnsService, historyState, urlHelper)
    {
        this.Type = dnsService.Type;
        this.Result = dnsService.Result;
    }
}