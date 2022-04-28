using System.ComponentModel.DataAnnotations;

namespace Statutis.Core.Form;

public class AuthenticationForm
{
    [Required(ErrorMessage = "Username required !")]
    public string Username { get; set; }= String.Empty;
    
    [Required(ErrorMessage = "Password required ! ")]
    public string Password { get; set; }= String.Empty;
}