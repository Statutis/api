using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Statutis.API.Models;
using Statutis.Core.Interfaces.Business.History;
using Statutis.Core.Interfaces.Business.Service;
using Statutis.Entity.History;

namespace Statutis.API.Controllers;

/// <summary>
/// Controleur sur l'historique des services
/// </summary>
[Tags("Historique")]
[Route("api/history")]
[ApiController]
public class HistoryController : Controller
{

	private readonly IHistoryEntryService _historyEntryService;
	private readonly IServiceService _serviceService;
	private readonly IGroupService _groupService;

	/// <summary>
	/// Constructeur
	/// </summary>
	/// <param name="historyEntryService"></param>
	/// <param name="serviceService"></param>
	/// <param name="groupService"></param>
	public HistoryController(IHistoryEntryService historyEntryService, IServiceService serviceService, IGroupService groupService)
	{
		_historyEntryService = historyEntryService;
		_serviceService = serviceService;
		_groupService = groupService;
	}

	/// <summary>
	/// Récupération de l'historique d'un service
	/// </summary>
	/// <param name="guid">Identifiant du service cible</param>
	/// <response code="404">Si le service visé n'existe pas.</response>
	/// <returns></returns>
	[HttpGet("service/{guid}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<HistoryEntryModel>))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Get([Required] Guid guid)
	{
		var service = await _serviceService.Get(guid);
		if (service == null)
			return NotFound();

		var history = await _historyEntryService.Get(service, 30);
		List<HistoryEntryModel> list = new List<HistoryEntryModel>();
		foreach (HistoryEntry historyEntry in history)
		{
			list.Add(new HistoryEntryModel(historyEntry, Url));
		}

		return Ok(list);
	}

	/// <summary>
	/// Récupération de l'historique des service d'un groupe
	/// </summary>
	/// <param name="guid">Identifiant du groupe cible</param>
	/// <response code="404">Si le groupe visé n'existe pas.</response>
	/// <returns></returns>
	[HttpGet("group/{guid}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<HistoryEntryModel>))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetFromAGroup([Required] Guid guid)
	{
		var group = await _groupService.Get(guid);
		if (group == null)
			return NotFound();

		var res = await _historyEntryService.GetHistoryEntryFromAGroup(group, ListSortDirection.Ascending);
		List<HistoryEntryModel> list = new List<HistoryEntryModel>();
		foreach (HistoryEntry historyEntry in res)
		{
			list.Add(new HistoryEntryModel(historyEntry, Url));
		}

		return Ok(list);
	}
}