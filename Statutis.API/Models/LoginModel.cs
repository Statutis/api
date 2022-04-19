namespace Statutis.API.Models;

public class LoginModel
{
    public LoginModel(string? token, bool status, string? msg, string? login, string? refresh)
    {
        Token = token;
        Status = status;
        Msg = msg;
        Login = login;
        Refresh = refresh;
    }

    public string? Token { get; set; }
    public bool Status { get; set; }
    public string? Msg { get; set; }
    public string? Login { get; set; }
    public string? Refresh { get; set; }
}