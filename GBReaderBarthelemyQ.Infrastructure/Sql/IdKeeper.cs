using GBReaderBarthelemyQ.Domains;

namespace GBReaderBarthelemyQ.Repository.Sql;

/// <summary>
/// Classe de stockage des id BD
/// </summary>
public class IdKeeper
{
    private readonly Dictionary<Book, int> _bookAssociation = new Dictionary<Book, int>();

    private readonly Dictionary<Page, int> _pageAssociation = new Dictionary<Page, int>();

    public void AddBook(Book book, int index)
    {
        if (!_bookAssociation.ContainsKey(book))
        {
            _bookAssociation.Add(book, index);
        }
    }

    public void AddPage(Page page, int index) => _pageAssociation.Add(page, index);

    public int GetBookId(Book book) => _bookAssociation[book];

    public int GetPageId(Page page) => _pageAssociation[page];

    public Page GetPageById(int id)
    {
        Page key = null!;
        foreach (KeyValuePair<Page, int> pair in _pageAssociation)
        {
            if (EqualityComparer<int>.Default.Equals(pair.Value, id))
            {
                key = pair.Key;
                break;
            }
        }
        return key;
    }
}