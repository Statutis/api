using System.ComponentModel;
using Statutis.Entity.History;
using Statutis.Entity.Service;

namespace Statutis.Core.Interfaces.DbRepository.History;

public interface IHistoryEntryRepository
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
	/// Récupéraion des entrées selon un service (par ordre décroissant)
	/// </summary>
	/// <param name="service"></param>
	/// <param name="count">Nombre limite d'entrée</param>
	/// <returns></returns>
	public Task<List<HistoryEntry>> Get(Entity.Service.Service service, int count = 15, ListSortDirection order = ListSortDirection.Descending);

	/// <summary>
	/// Ajout d'une entrée
	/// </summary>
	/// <param name="historyEntry"></param>
	/// <returns></returns>
	public Task<HistoryEntry> Add(HistoryEntry historyEntry);

	/// <summary>
	/// Récupération de la dernière entrée pour chaque service
	/// </summary>
	/// <param name="services"></param>
	/// <returns></returns>
	public Task<Dictionary<Entity.Service.Service, HistoryEntry?>> GetAllLast(List<Entity.Service.Service> services);




}