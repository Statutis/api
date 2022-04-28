using System.ComponentModel.DataAnnotations;

namespace Statutis.API.Form;

public class ServiceForm
{
    [Required]
    public string Name { get; set; }= String.Empty;
    [Required]
    public string GroupRef { get; set; }= String.Empty;
    [Required]
    public string Description { get; set; }= String.Empty;
    [Required]
    public string Host { get; set; }= String.Empty;
    [Required]
    public string ServiceTypeRef { get; set; }= String.Empty;
    
}