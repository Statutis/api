using Microsoft.AspNetCore.Mvc;

namespace Statutis.API.Models;

/// <summary>
/// Model sur l'authentification
/// </summary>
public class AuthModel
{
    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="urlHelper"></param>
    public AuthModel(string? msg, IUrlHelper urlHelper)
    {
        Login = urlHelper.Action("Login","Authenticate");
        Refresh = urlHelper.Action("Refresh", "Authenticate");
        Register = urlHelper.Action("Register", "Authenticate");
        Msg = msg;
    }
    
    /// <summary>
    /// Messsage
    /// </summary>
    public string? Msg { get; set; }
    
    /// <summary>
    /// Reférence sur l'url de login
    /// </summary>
    public string? Login { get; set; }
    
    /// <summary>
    /// Reférence sur l'url de renouvellement du token
    /// </summary>
    public string? Refresh { get; set; }
    
    /// <summary>
    /// Reférence sur l'url d'enregistrement
    /// </summary>
    public string? Register { get; set; }
}