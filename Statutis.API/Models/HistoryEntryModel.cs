using Microsoft.AspNetCore.Mvc;
using Statutis.Entity.History;
using Statutis.Entity.Service;

namespace Statutis.API.Models;

/// <summary>
/// Model pour une entreé d'historique
/// </summary>
public class HistoryEntryModel
{
	/// <summary>
	/// Numéro du service cible
	/// </summary>
	public Guid ServiceId { get; set; }

	/// <summary>
	/// Date de génération
	/// </summary>
	public DateTime DateTime { get; set; }

	/// <summary>
	/// Etat résultant 
	/// </summary>
	public HistoryState State { get; set; }

	/// <summary>
	/// Message (ex : Erreur)
	/// </summary>
	public String? Message { get; set; } = null;

	/// <summary>
	/// Référence vers le Service cible
	/// </summary>
	public String ServiceRef { get; set; }

	/// <summary>
	/// Constructeur
	/// </summary>
	/// <param name="historyEntry"></param>
	/// <param name="urlHelper"></param>
	public HistoryEntryModel(HistoryEntry historyEntry, IUrlHelper urlHelper)
	{
		ServiceId = historyEntry.ServiceId;
		DateTime = historyEntry.DateTime;
		State = historyEntry.State;
		Message = historyEntry.message;

		ServiceRef = urlHelper.Action("Get", "Service", new { Guid = ServiceId }) ?? String.Empty;
	}
}