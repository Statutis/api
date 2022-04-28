using System.ComponentModel.DataAnnotations;

namespace Statutis.API.Form;

/// <summary>
/// Fomulaire pour les groupes
/// </summary>
public class GroupForm
{
	/// <summary>
	/// Nom du groupe
	/// </summary>
	[StringLength(maximumLength: 30), Required]
	public String Name { get; set; }= String.Empty;

	
	/// <summary>
	/// Description du groupe
	/// </summary>
	public String Description { get; set; }= String.Empty;

	/// <summary>
	/// Liste des références des équipes
	/// </summary>
	public List<String> Teams { get; set; } = new List<String>();

	/// <summary>
	/// Visilibité
	/// </summary>
	public bool IsPublic { get; set; } = true;
}