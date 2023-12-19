using GBReaderBarthelemyQ.Domains;

namespace GBReaderBarthelemyQ.Repositories;

/// <summary>
/// Interface de repository
/// </summary>
public interface IDataInterface : IDisposable
{
    /// <summary>
    /// Récupère une liste de livre sur base d'une query
    /// </summary>
    /// <param name="searchString">(String) chaine de caractère de recherche</param>
    /// <returns>(IReadOnlyList<Book>) Liste de livres (retourne des livres aléatoires si searchString est vide ou null)</returns>
    IReadOnlyList<Book> LoadBooks(String searchString);

    /// <summary>
    /// Récupère une liste de livre sur des isbns
    /// </summary>
    /// <param name="isbns">(string[]) tableau d'isbn string</param>
    /// <returns>(IReadOnlyList<Book>) Liste de livres</returns>
    IReadOnlyList<Book> LoadBooksByIsbns(string[] isbns);

    /// <summary>
    /// Charge les pages et les choix du livre
    /// </summary>
    /// <param name="book">(Book) livre</param>
    void LoadPages(Book book);
}