namespace Statutis.API.Form;

/// <summary>
/// Fomulaire de modification sur l'utilisateur
/// </summary>
public class UserPatchModel
{
	/// <summary>
	/// Nom de l'utilisateur
	/// </summary>
	public string? Username { get; set; }
	
	/// <summary>
	/// Nom d'utilisateur
	/// </summary>
	public string? Name { get; set; }
	
	/// <summary>
	/// Prènom de l'utilisateur
	/// </summary>
	public string? Firstname { get; set; }
	
	/// <summary>
	/// Mot de passe de l'utilisateur
	/// </summary>
	public string? Password { get; set; }
}