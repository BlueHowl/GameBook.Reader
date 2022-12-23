using System.Collections.ObjectModel;

namespace GBReaderBarthelemyQ.Domains;

/// <summary>
/// Classe page
/// </summary>
public class Page
{
    public string Text { get; }

    public List<Choice> Choices { get; }

    /// <summary>
    /// Constructeur de la classe page
    /// </summary>
    /// <param name="text">(String) texte de la page</param>
    /// <param name="choices">(Collection<Choice>) liste de choix</param>
    public Page(string text, Collection<Choice> choices)
    {
        Text = text;
        Choices = (choices != null) ? new List<Choice>(choices) : new List<Choice>();
    }
}