using System.ComponentModel.DataAnnotations;

namespace Statutis.API.Form;

public class ServiceForm
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string GroupRef { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string Host { get; set; }
    [Required]
    public string ServiceTypeRef { get; set; }
    
}