using System.ComponentModel.DataAnnotations;

namespace Statutis.API.Form;

public class HttpForm : ServiceForm
{
    [Required]
    public int Port { get; set; }

    public int? Code { get; set; } = null;

    public String? RedirectUrl { get; set; } = null;
}