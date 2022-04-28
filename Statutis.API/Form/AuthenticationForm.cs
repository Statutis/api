using System.ComponentModel.DataAnnotations;

namespace Statutis.API.Form;


/// <summary>
/// Formulaire d'authentification
/// </summary>
public class AuthenticationForm
{
    /// <summary>
    /// Nom d'utilisateur
    /// </summary>
    [Required(ErrorMessage = "Username required !")]
    public string Username { get; set; }= String.Empty;
    
    /// <summary>
    /// Mot de passe
    /// </summary>
    [Required(ErrorMessage = "Password required ! ")]
    public string Password { get; set; }= String.Empty;
}