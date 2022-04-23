using System.ComponentModel.DataAnnotations;

namespace Statutis.Core.Form;

public class RegistrationForm
{
	[Required, MinLength(3)]
	public string Username { get; set; }

	[Required, MinLength(3)]
	public string Name { get; set; }

	[Required, MinLength(3)]
	public string Firstname { get; set; }

	[Required, MinLength(8)]
	public string Password { get; set; }

	[Required, EmailAddress]
	public string Email { get; set; }
}