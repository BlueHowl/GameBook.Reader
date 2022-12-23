using GBReaderBarthelemyQ.Domains;
using GBReaderBarthelemyQ.Presenters.Events;
using GBReaderBarthelemyQ.Presenters.Interfaces;
using GBReaderBarthelemyQ.Presenters.Interfaces.Notifications;
using GBReaderBarthelemyQ.Presenters.Interfaces.Pages;
using GBReaderBarthelemyQ.Repositories;
using GBReaderBarthelemyQ.Repositories.Exceptions;

namespace GBReaderBarthelemyQ.Presenters;

/// <summary>
/// Classe présentateur de liste de livre
/// </summary>
public class BookListPresenter
{

    private readonly IBookListView _view;

    private readonly INotificationAndRoute _notificationAndRoute;

    private readonly IDataFactoryInterface _factory;

    private readonly Library _library;

    private readonly ISessionRepository _sessionRepository;

    /// <summary>
    /// Constructeur du présentateur de liste de livre
    /// </summary>
    /// <param name="view"></param>
    /// <param name="notificationAndRoute"></param>
    /// <param name="factory"></param>
    /// <param name="library"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public BookListPresenter(IBookListView view, INotificationAndRoute notificationAndRoute, IDataFactoryInterface factory, Library library, ISessionRepository sessionRepository)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
        _notificationAndRoute = notificationAndRoute ?? throw new ArgumentNullException(nameof(notificationAndRoute));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _library = library ?? throw new ArgumentNullException(nameof(library));
        _sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));

        _view.FilterRequested += OnFilterRequested;
        _view.StartReadRequested += OnStartReadRequested;
        _view.StatsRequested += OnStatsRequested;

        _view.DisplayBooks(GetBookList(null!)); //todo change

    }

    /// <summary>
    /// Récupère les livres sur base d'une requête
    /// </summary>
    /// <param name="searchString">(string) chaine de caractère de recherche</param>
    /// <returns>(List<BookModel>) liste de livres à afficher (retourne des livres aléatoires si searchString est vide ou null)</returns>
    private List<Book> GetBookList(string searchString)
    {
        Dictionary<Isbn, Session> sessionsMap = _sessionRepository.GetSessions();

        string[] isbns = sessionsMap.Keys.Select(i => i.IsbnNumber).ToArray();

        List<Book> books = new List<Book>();

        try
        {
            IDataInterface repository = _factory.NewStorage();

            if (searchString == null && isbns.Length != 0)
            {
                books.AddRange(repository.LoadBooksByIsbns(isbns));
            }
            books = books.Union(repository.LoadBooks(searchString!)).ToList();

            _library.Books = new List<Book>(books);

            foreach (Book book in books)
            {
                if (sessionsMap.ContainsKey(book.BookCover.Isbn))
                {
                    book.Session = sessionsMap[book.BookCover.Isbn];
                }
            }
        }
        catch (UnableToConnectException e)
        {
            _notificationAndRoute.Push(NotificationSeverity.Error, "Erreur :", e.Message);
        }
        catch (Exception e)
        {
            _notificationAndRoute.Push(NotificationSeverity.Warning, "Attention :", e.Message);
        }

        return books;
    }

    /// <summary>
    /// Est appelé via un click Bouton Filter
    /// Récupère et affiche les livres avec un filtre
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnFilterRequested(object? sender, FilterEventArgs e) => _view.DisplayBooks(GetBookList(e.searchString));

    /// <summary>
    /// Est appelé lors de l'appui sur le bouton de lecture du livre
    /// change l'isbn du livre courant
    /// Charge la page d'affichage des pages
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnStartReadRequested(object? sender, StartReadEventArgs e)
    {
        _library.CurrentBookIsbn = e.BookIsbn;
        _notificationAndRoute.GoTo("PageDisplay");
    }

    /// <summary>
    /// Est appelé lors de l'appui sur le bouton consulter stats
    /// Charge la page d'affichage des sessions
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnStatsRequested(object? sender, EventArgs e) => _notificationAndRoute.GoTo("Stats");
}