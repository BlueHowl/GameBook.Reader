using System.Collections.ObjectModel;

namespace GBReaderBarthelemyQ.Domains;

/// <summary>
/// Classe bibliothéque
/// </summary>
public class Library
{
    private List<Book>? _books;

    private Isbn? _currentBookIsbn;

    public List<Book> Books
    {
        get => _books!;
        set => _books = value;
    }

    public Isbn CurrentBookIsbn
    {
        get => _currentBookIsbn!;
        set => _currentBookIsbn = value;
    }

    /// <summary>
    /// Constructeur de la bibliothéque
    /// </summary>
    /// <param name="books"></param>
    public Library(Collection<Book> books)
    {
        Books = (books != null) ? new List<Book>(books) : new List<Book>();
    }

    /// <summary>
    /// Récupère le livre courant
    /// </summary>
    /// <returns></returns>
    public Book GetCurrentBook()
    {
        foreach (Book book in _books!)
        {
            if (book.BookCover.Isbn.Equals(CurrentBookIsbn)) //TODO Demeter
            {
                return book;
            }
        }
        return null!;
    }

}