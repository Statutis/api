using System.ComponentModel.DataAnnotations;

namespace Statutis.API.Form;

public class DnsForm : ServiceForm
{
    [Required]
    public string Type { get; set; }= String.Empty;
    [Required]
    public string Result { get; set; }= String.Empty;
}