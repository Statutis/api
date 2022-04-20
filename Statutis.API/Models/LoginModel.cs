using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;

namespace Statutis.API.Models;

public class LoginModel : AuthModel
{
    public LoginModel(string? token, bool status, string? msg, IUrlHelper urlHelper) : base(msg, urlHelper)
    {
        Token = token;
        Status = status;
    }

    public string? Token { get; set; }
    public bool Status { get; set; }
}