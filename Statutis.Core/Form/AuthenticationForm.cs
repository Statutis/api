using System.ComponentModel.DataAnnotations;

namespace Statutis.Core.Form;

public class AuthenticationForm
{
    [Required(ErrorMessage = "Username required !")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Password required ! ")]
    public string Password { get; set; }
}