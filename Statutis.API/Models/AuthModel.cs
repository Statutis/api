using Microsoft.AspNetCore.Mvc;

namespace Statutis.API.Models;

public class AuthModel
{
    public AuthModel(string? msg, IUrlHelper urlHelper)
    {
        Login = urlHelper.Action("Login","Authenticate");
        Refresh = urlHelper.Action("Refresh", "Authenticate");
        Register = urlHelper.Action("Register", "Authenticate");
        Msg = msg;
    }
    
    public string? Msg { get; set; }
    
    public string? Login { get; set; }
    public string? Refresh { get; set; }
    
    public string? Register { get; set; }
}