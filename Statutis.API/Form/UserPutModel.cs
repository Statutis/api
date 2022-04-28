using System.ComponentModel.DataAnnotations;

namespace Statutis.API.Form;

public class UserPutModel
{
	[Required, MinLength(3)]
	public string Username { get; set; } = String.Empty;

	[Required, MinLength(3)]
	public string Name { get; set; } = String.Empty;

	[Required, MinLength(3)]
	public string Firstname { get; set; } = String.Empty;

	[Required, MinLength(8)]
	public string Password { get; set; } = String.Empty;
}