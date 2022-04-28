using System.ComponentModel.DataAnnotations;

namespace Statutis.API.Form;

/// <summary>
/// Formulaire pour les serices de Type DNS
/// </summary>
public class DnsForm : ServiceForm
{
    /// <summary>
    /// Type de requêtes DNS
    /// </summary>
    /// <example>A</example>
    /// <example>NS</example>
    /// <example>PTR</example>
    /// <example>MX</example>
    [Required]
    public string Type { get; set; }= String.Empty;
    
    /// <summary>
    /// Retour attendu par la requête
    /// </summary>
    [Required]
    public string Result { get; set; }= String.Empty;
}