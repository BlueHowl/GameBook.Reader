using GBReaderBarthelemyQ.Domains;
using GBReaderBarthelemyQ.Repository.Dto;
using GBReaderBarthelemyQ.Repository.Json.Dto;

namespace GBReaderBarthelemyQ.Repository.Json;

/// <summary>
/// Classe de mapping DTO
/// </summary>
public class Mapper
{

    /// <summary>
    /// Converti l'objet livre en objet DTO Livre
    /// </summary>
    /// <param name="book">(Book) livre</param>
    /// <returns>(BookDTO) livre dto</returns>
    public static BookDTO BookToDto(Book book)
    {
        Cover cover = book.BookCover;
        return new BookDTO(cover.Title, cover.Summary, cover.Author, cover.Isbn.IsbnNumber);
    }

    /// <summary>
    /// Converti l'objet DTO livre en objet livre
    /// </summary>
    /// <param name="bookDto">(BookDTO) livre dto</param>
    /// <returns>(Book) livre</returns>
    public static Book DtoToBook(BookDTO bookDto) => new Book(new Cover(bookDto.Title, bookDto.Summary, bookDto.Author, new Isbn(bookDto.Isbn)), null!);


    /// <summary>
    /// Converti les objets Session en objets DTO Session
    /// </summary>
    /// <param name="session">(Dictionary<Isbn, Session>) SessionsMap</param>
    /// <returns>(IReadOnlyList<SessionDTO>) sessions dto</returns>
    public static IReadOnlyList<SessionDTO> SessionToDto(Dictionary<Isbn, Session> sessionsMap)
    {
        List<SessionDTO> sessionsDto = new List<SessionDTO>();

        foreach (var session in sessionsMap)
        {
            if (session.Value != null)
            {
                sessionsDto.Add(new SessionDTO(session.Key.IsbnNumber, session.Value.Start, session.Value.Last, session.Value.LastPageNum));
            }
        }

        return sessionsDto;
    }

    /// <summary>
    /// Converti les objets DTO session en objets session
    /// </summary>
    /// <param name="sessionDto">(IReadOnlyList<SessionDTO>) sessions dto</param>
    /// <returns>(Dictionary<Isbn, Session>) sessionsMap</returns>
    public static Dictionary<Isbn, Session> DtoToSession(IReadOnlyList<SessionDTO> sessionsDto)
    {
        Dictionary<Isbn, Session> sessionsMap = new Dictionary<Isbn, Session>();

        if (sessionsDto != null)
        {
            foreach (var sessionDto in sessionsDto)
            {
                Session session = new Session(sessionDto.Start, sessionDto.Last, sessionDto.LastPageNum);
                sessionsMap.Add(new Isbn(sessionDto.Isbn), session);
            }
        }

        return sessionsMap;
    }

}