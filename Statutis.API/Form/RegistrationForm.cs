using System.ComponentModel.DataAnnotations;

namespace Statutis.API.Form;

/// <summary>
/// Formulaire d'enregistrement
/// </summary>
public class RegistrationForm
{
	/// <summary>
	/// Nom d'utilisateur
	/// </summary>
	[Required, MinLength(3)]
	public string Username { get; set; }= String.Empty;
	
	/// <summary>
	/// Nom de l'utilisateur 
	/// </summary>
	[Required, MinLength(3)]
	public string Name { get; set; }= String.Empty;

	/// <summary>
	/// Prénom de l'utilisateur
	/// </summary>
	[Required, MinLength(3)]
	public string Firstname { get; set; }= String.Empty;

	/// <summary>
	/// Mot de passe de l'utilisateur
	/// </summary>
	[Required, MinLength(8)]
	public string Password { get; set; }= String.Empty;
	
	/// <summary>
	/// Adresse mail de l'utilisateur 
	/// </summary>
	[Required, EmailAddress]
	public string Email { get; set; }= String.Empty;
}