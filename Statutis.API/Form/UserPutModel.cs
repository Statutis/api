using System.ComponentModel.DataAnnotations;

namespace Statutis.API.Form;


/// <summary>
/// Fomulaire de modification de l'utilisateur
/// </summary>
public class UserPutModel
{
	
	/// <summary>
	/// Nom d'utilisateur
	/// </summary>
	[Required, MinLength(3)]
	public string Username { get; set; } = String.Empty;

	
	/// <summary>
	/// Nom de l'utilisateur
	/// </summary>
	[Required, MinLength(3)]
	public string Name { get; set; } = String.Empty;

	/// <summary>
	/// Prènom de l'utilisateur
	/// </summary>
	[Required, MinLength(3)]
	public string Firstname { get; set; } = String.Empty;

	/// <summary>
	/// Mot de passe de l'utilisateur
	/// </summary>
	[Required, MinLength(8)]
	public string Password { get; set; } = String.Empty;
}