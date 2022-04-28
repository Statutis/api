using System.ComponentModel.DataAnnotations;

namespace Statutis.Core.Form;

public class RegistrationForm
{
	[Required, MinLength(3)]
	public string Username { get; set; }= String.Empty;

	[Required, MinLength(3)]
	public string Name { get; set; }= String.Empty;

	[Required, MinLength(3)]
	public string Firstname { get; set; }= String.Empty;

	[Required, MinLength(8)]
	public string Password { get; set; }= String.Empty;

	[Required, EmailAddress]
	public string Email { get; set; }= String.Empty;
}