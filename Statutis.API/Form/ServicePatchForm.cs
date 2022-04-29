using System.ComponentModel.DataAnnotations;

namespace Statutis.API.Form;

/// <summary>
/// Formulaire sur les services 
/// </summary>
public class ServicePatchForm
{
    /// <summary>
    /// </summary>
    [Required]
    public Guid Guid { get; set; }

    /// <summary>
    /// Nom du service
    /// </summary>
    [Required]
    public string Name { get; set; } = String.Empty;

    /// <summary>
    ///Référence sur le groupe 
    /// </summary>
    [Required]
    public string GroupRef { get; set; } = String.Empty;

    /// <summary>
    /// Description sur le groupe
    /// </summary>
    [Required]
    public string Description { get; set; } = String.Empty;

    /// <summary>
    /// Hôte cible
    /// </summary>
    [Required]
    public string Host { get; set; } = String.Empty;

    /// <summary>
    /// Référence sur le type de service
    /// </summary>
    [Required]
    public string ServiceTypeRef { get; set; } = String.Empty;
}