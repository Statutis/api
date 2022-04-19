using Statutis.Entity.History;
using Statutis.Entity.Service;

namespace Statutis.Core.Interfaces.Business.History;

public interface IHistoryEntryService
{
	/// <summary>
	/// Dernière entré pour ce service
	/// </summary>
	/// <param name="service"></param>
	/// <returns></returns>
	public Task<HistoryEntry?> GetLast(Entity.Service.Service service);

	/// <summary>
	/// Dernière entré avec cet état pour ce service  
	/// </summary>
	/// <param name="service"></param>
	/// <param name="state"></param>
	/// <returns></returns>
	public Task<HistoryEntry?> GetLast(Entity.Service.Service service, HistoryState state);
	
	/// <summary>
	/// Récupération de la dernière entrée pour chaque service
	/// </summary>
	/// <returns></returns>
	public Task<Dictionary<Entity.Service.Service, HistoryEntry?>> GetAllLast();
	
	/// <summary>
	/// Récupération de la dernière entrée pour chaque service
	/// </summary>
	/// <returns></returns>
	public Task<Dictionary<Entity.Service.Service, HistoryEntry?>> GetAllLast(List<Entity.Service.Service> services);

	/// <summary>
	/// Récupéraion des entrées selon un service (par ordre décroissant)
	/// </summary>
	/// <param name="service"></param>
	/// <param name="count">Nombre limite d'entrée</param>
	/// <returns></returns>
	public Task<List<HistoryEntry>> Get(Entity.Service.Service service, int count = 15);

	/// <summary>
	/// Ajout d'une entrée
	/// </summary>
	/// <param name="historyEntry"></param>
	/// <returns></returns>
	public Task<HistoryEntry> Add(HistoryEntry historyEntry);

	/// <summary>
	/// Récupération de l'états global
	/// </summary>
	/// <returns></returns>
	public Task<Tuple<HistoryState, DateTime>> GetMainState();
}