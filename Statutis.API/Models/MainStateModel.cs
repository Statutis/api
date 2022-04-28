using Statutis.Entity.History;

namespace Statutis.API.Models;

/// <summary>
/// Modèle pour l'état global
/// </summary>
public class MainStateModel
{
	/// <summary>
	/// Dernière mise à jour
	/// </summary>
	public DateTime LastUpdate { get; set; }
	
	
	/// <summary>
	/// Etat global
	/// </summary>
	public HistoryState State { get; set; }
}