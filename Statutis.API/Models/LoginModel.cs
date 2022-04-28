using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;

namespace Statutis.API.Models;

/// <summary>
/// Modèle pour la connexion
/// </summary>
public class LoginModel : AuthModel
{
    
    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="token"></param>
    /// <param name="status"></param>
    /// <param name="msg"></param>
    /// <param name="urlHelper"></param>
    public LoginModel(string? token, bool status, string? msg, IUrlHelper urlHelper) : base(msg, urlHelper)
    {
        Token = token;
        Status = status;
    }

    /// <summary>
    /// Token de connexion
    /// </summary>
    public string? Token { get; set; }
    
    /// <summary>
    /// Status
    /// </summary>
    /// <returns>True : succés, False : erreur</returns>
    public bool Status { get; set; }
}