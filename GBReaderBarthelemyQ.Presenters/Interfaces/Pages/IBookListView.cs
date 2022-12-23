using GBReaderBarthelemyQ.Domains;
using GBReaderBarthelemyQ.Presenters.Events;

namespace GBReaderBarthelemyQ.Presenters.Interfaces.Pages;

/// <summary>
/// Interface de vue d'affichage de liste des livres
/// </summary>
public interface IBookListView : IStartReadEvent
{
    void DisplayBooks(List<Book> books);

    event EventHandler<FilterEventArgs> FilterRequested;

    event EventHandler StatsRequested;
}