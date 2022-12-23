using GBReaderBarthelemyQ.Presenters.Events;
using GBReaderBarthelemyQ.Presenters.ViewModels;

namespace GBReaderBarthelemyQ.Presenters.Interfaces.Pages;

/// <summary>
/// Interface de vue d'affichage de page
/// </summary>
public interface IPageDisplayView
{
    void DisplayPage(PageViewModel pageViewModel);

    event EventHandler<NextPageEventArgs> NextPageRequested;

    event EventHandler GoToBookListRequested;

    event EventHandler<RestartReadEventArgs> StartReadRequested;
}