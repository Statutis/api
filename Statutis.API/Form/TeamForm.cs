using System.ComponentModel.DataAnnotations;

namespace Statutis.API.Form;

/// <summary>
/// Fomulaire sur les équipes
/// </summary>
public class TeamForm
{
	
	/// <summary>
	/// Nom de l'équipe
	/// </summary>
	public String Name { get; set; }= String.Empty;

	
	/// <summary>
	/// Couleur de l'équipe
	/// </summary>
	[StringLength(maximumLength: 10)]
	public String? Color { get; set; } = null;
	
	/// <summary>
	/// Liste de référence d'utilisateurs
	/// </summary>
	public List<String> Users { get; set; } = new List<String>();
}