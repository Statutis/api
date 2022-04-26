using System.ComponentModel.DataAnnotations;

namespace Statutis.API.Form;

public class DnsForm : ServiceForm
{
    [Required]
    public string Type { get; set; }
    [Required]
    public string Result { get; set; }
}