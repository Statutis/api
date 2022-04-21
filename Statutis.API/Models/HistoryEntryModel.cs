using Microsoft.AspNetCore.Mvc;
using Statutis.Entity.History;
using Statutis.Entity.Service;

namespace Statutis.API.Models;

public class HistoryEntryModel
{
    public Guid ServiceId { get; set; }
    public DateTime DateTime { get; set; }
    public HistoryState State { get; set; }
    public String? Message { get; set; } = "";
    
    public String ServiceRef { get; set; }

    public HistoryEntryModel(HistoryEntry historyEntry, IUrlHelper urlHelper)
    {
        ServiceId = historyEntry.ServiceId;
        DateTime = historyEntry.DateTime;
        State = historyEntry.State;
        Message = historyEntry.message;

        ServiceRef = urlHelper.Action("Get", "Service", new {Guid = ServiceId}) ?? String.Empty;
    }
}