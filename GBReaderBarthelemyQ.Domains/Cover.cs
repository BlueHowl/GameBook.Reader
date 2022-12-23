namespace GBReaderBarthelemyQ.Domains;


/// <summary>
/// Record couverture
/// </summary>
/// <param name="Title">(string) titre du livre</param>
/// <param name="Summary">(string) résumé du livre</param>
/// <param name="Author">(string) nom et prénom de l'auteur</param>
/// <param name="Isbn">(Isbn) numéro isbn unique du livre</param>
public record Cover(string Title, string Summary, string Author, Isbn Isbn);