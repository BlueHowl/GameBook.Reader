namespace GBReaderBarthelemyQ.Domains;

/// <summary>
/// Classe choix
/// </summary>
/// <param name="Text">(String) texte du choix</param>
/// <param name="PageRef">(int) numéro de la page à rediriger</param>
public record Choice(string Text, int PageRef);