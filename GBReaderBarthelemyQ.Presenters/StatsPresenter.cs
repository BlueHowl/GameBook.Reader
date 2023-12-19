using GBReaderBarthelemyQ.Domains;
using GBReaderBarthelemyQ.Presenters.Interfaces;
using GBReaderBarthelemyQ.Presenters.Interfaces.Pages;

namespace GBReaderBarthelemyQ.Presenters;
public class StatsPresenter
{
    private readonly IStatsView _view;

    private readonly INotificationAndRoute _notificationAndRoute;

    private readonly Library _library;

    public StatsPresenter(IStatsView view, INotificationAndRoute notificationAndRoute, Library library)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
        _notificationAndRoute = notificationAndRoute ?? throw new ArgumentNullException(nameof(notificationAndRoute));
        _library = library ?? throw new ArgumentNullException(nameof(library));

        notificationAndRoute.StatsRequested += OnStatsRequested;
        _view.BackRequested += OnBackRequested;
    }

    private void OnStatsRequested(object? sender, EventArgs e) => _view.DisplaySession(_library.Books);

    /// <summary>
    /// Est appelé lors de l'appui sur le bouton retour
    /// Charge la page d'affichage des livres
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnBackRequested(object? sender, EventArgs e) => _notificationAndRoute.GoTo("BookList");
}