using System.Collections.ObjectModel;

namespace GBReaderBarthelemyQ.Domains;

/// <summary>
/// Classe livre
/// </summary>
public class Book
{
    private Cover? _bookCover;

    private List<Page>? _pages;

    private Session? _session;

    public Cover BookCover
    {
        get => _bookCover!;
        set => _bookCover = value;
    }

    public List<Page> Pages
    {
        get => _pages!;
        set => _pages = value;
    }

    public Session Session
    {
        get => _session!;
        set => _session = value;
    }

    /// <summary>
    /// Constructeur classe livre
    /// </summary>
    /// <param name="cover">(Cover) couverture du livre</param>
    /// <param name="pages">(Collection<Page>) pages du livre</param>
    public Book(Cover cover, Collection<Page> pages)
    {
        BookCover = cover;
        Pages = (pages != null) ? new List<Page>(pages) : new List<Page>();
    }

    public override bool Equals(object? obj) => obj is Book book && BookCover.Isbn.Equals(book.BookCover.Isbn);

    public override int GetHashCode() => HashCode.Combine(BookCover.Isbn);
}