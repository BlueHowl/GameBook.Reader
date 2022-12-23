using GBReaderBarthelemyQ.Domains;

namespace GBReaderBarthelemyQ.Presenters.ViewModels;

/// <summary>
/// Record PageViewModel
/// </summary>
/// <param name="PageNumber">(int) Numéro de la page</param>
/// <param name="Text">(String) texte</param>
/// <param name="Choices">(String) liste de choix pour vue</param>
public record struct PageViewModel(int PageNumber, string Text, List<Choice> Choices)
{
    public int PageNumber { get; init; } = PageNumber;
    public string Text { get; init; } = Text;
    public List<Choice> Choices { get; init; } = Choices;

    public List<string> ChoicesText => new List<string>(Choices.Select(c => c.Text).ToList());
}