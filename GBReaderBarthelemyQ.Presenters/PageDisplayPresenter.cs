using GBReaderBarthelemyQ.Domains;
using GBReaderBarthelemyQ.Presenters.Events;
using GBReaderBarthelemyQ.Presenters.Interfaces;
using GBReaderBarthelemyQ.Presenters.Interfaces.Notifications;
using GBReaderBarthelemyQ.Presenters.Interfaces.Pages;
using GBReaderBarthelemyQ.Presenters.ViewModels;
using GBReaderBarthelemyQ.Repositories;
using GBReaderBarthelemyQ.Repositories.Exceptions;

namespace GBReaderBarthelemyQ.Presenters;

/// <summary>
/// Classe présentateur d'affichage des pages
/// </summary>
public class PageDisplayPresenter
{
    private readonly IPageDisplayView _view;

    private readonly INotificationAndRoute _notificationAndRoute;

    private readonly IDataFactoryInterface _factory;

    private readonly Library _library;

    /// <summary>
    /// Constructeur de présentateur de pages
    /// </summary>
    /// <param name="view"></param>
    /// <param name="notificationAndRoute"></param>
    /// <param name="factory"></param>
    /// <param name="library"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public PageDisplayPresenter(IPageDisplayView view, INotificationAndRoute notificationAndRoute, IDataFactoryInterface factory, Library library)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
        _notificationAndRoute = notificationAndRoute ?? throw new ArgumentNullException(nameof(notificationAndRoute));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _library = library ?? throw new ArgumentNullException(nameof(library));

        _notificationAndRoute.StartReadRequested += OnStartReadRequested;
        _view.StartReadRequested += OnStartReadRequested;
        _view.NextPageRequested += OnNextPageRequested;
        _view.GoToBookListRequested += OnGoToBookListRequested;
    }

    /// <summary>
    /// Charge les pages dans la bibliothéque
    /// </summary>
    private void LoadPages()
    {
        try
        {
            IDataInterface repository = _factory.NewStorage();
            repository.LoadPages(_library.GetCurrentBook());
        }
        catch (UnableToConnectException e)
        {
            _notificationAndRoute.Push(NotificationSeverity.Error, "Erreur :", e.Message);
        }
        catch (Exception e)
        {
            _notificationAndRoute.Push(NotificationSeverity.Warning, "Attention :", e.Message);
        }
    }

    /// <summary>
    /// Récupère la page et l'affiche
    /// </summary>
    /// <param name="pageNum">Numéro de page</param>
    public void ShowPage(int pageNum)
    {
        Book currentBook = _library.GetCurrentBook();

        currentBook.Session.UpdateLastState(pageNum); //todo demeter
        PageViewModel pageViewModel = GetPage(pageNum);

        _view.DisplayPage(pageViewModel);

        if (pageViewModel.Choices.Count == 0)
        {
            currentBook.Session = null!;
        }
    }

    /// <summary>
    /// Récupère la page à afficher
    /// </summary>
    /// <param name="pageNum">(int) numéro de page</param>
    /// <returns>(PageViewModel) modèle de vue page</returns>
    private PageViewModel GetPage(int pageNum)
    {
        Book book = _library.GetCurrentBook();

        List<Choice> choices = new List<Choice>();
        //TODO check cas dégradé when no pages in book
        foreach (Choice choice in book.Pages[pageNum].Choices)
        {
            choices.Add(choice);
        }

        return new PageViewModel(pageNum + 1, book.Pages[pageNum].Text, choices);
    }

    /// <summary>
    /// Est appelé lors du changement de vue ou lors de l'appui du bouton RestartRead
    /// Charge les pages si pas déjà fait et affiche la premiere page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnStartReadRequested(object? sender, RestartReadEventArgs e)
    {
        Book currentBook = _library.GetCurrentBook();
        if (currentBook.Pages.Count == 0) //TODO demeter
        {
            LoadPages();
        }

        if (currentBook.Session == null || e.restart)
        {
            currentBook.Session = new Session();
        }

        ShowPage(currentBook.Session.LastPageNum);
    }

    /// <summary>
    /// Est appelé lors de la sélection d'un choix
    /// Affiche la page de destination du choix
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnNextPageRequested(object? sender, NextPageEventArgs e) => ShowPage(e.pageIndex);

    /// <summary>
    /// Est appelé lors de l'appui sur le bouton GoToBookList
    /// Charge la page liste de livres
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnGoToBookListRequested(object? sender, EventArgs e) => _notificationAndRoute.GoTo("BookList");

}