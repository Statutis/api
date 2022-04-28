using System.ComponentModel.DataAnnotations;

namespace Statutis.API.Form;

/// <summary>
/// Formulaire pour les services en mode de vérification HTTP
/// </summary>
public class HttpForm : ServiceForm
{

    /// <summary>
    /// Code de retour attendu
    /// </summary>
    public int? Code { get; set; } = null;

    /// <summary>
    /// Url de redirection
    /// </summary>
    public String? RedirectUrl { get; set; } = null;
}