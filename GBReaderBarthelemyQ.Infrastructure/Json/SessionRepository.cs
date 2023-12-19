using GBReaderBarthelemyQ.Domains;
using GBReaderBarthelemyQ.Repositories;
using GBReaderBarthelemyQ.Repositories.Exceptions;
using GBReaderBarthelemyQ.Repository.Json.Dto;
using Newtonsoft.Json;
using System.Text;

namespace GBReaderBarthelemyQ.Repository.Json;

/// <summary>
/// Classe json repository
/// </summary>
public class SessionRepository : ISessionRepository

{
    private readonly string _JsonBooksPath;

    /// <summary>
    /// Constructeur du repository json
    /// </summary>
    public SessionRepository(string path)
    {
        _JsonBooksPath = path;
    }

    /// <summary>
    /// Récupère les sessions sauvegardées
    /// </summary>
    /// <returns>(IReadOnlyList<Session>) liste de sessions en lecture seule</returns>
    /// <exception cref="UnableToLoadException"></exception>
    public Dictionary<Isbn, Session> GetSessions()
    {
        try
        {
            string jsonSessions = string.Empty;

            if (File.Exists(_JsonBooksPath))
            {
                jsonSessions = File.ReadAllText(_JsonBooksPath);
            }

            IReadOnlyList<SessionDTO> sessionsDto = JsonConvert.DeserializeObject<IReadOnlyList<SessionDTO>>(jsonSessions)!;

            return Mapper.DtoToSession(sessionsDto!);
        }
        catch (JsonSerializationException e)
        {
            throw new UnableToLoadException("Erreur de format des sessions", e);
        }
        catch (JsonReaderException e)
        {
            throw new UnableToLoadException("Erreur lors du chargement du fichier json", e);
        }
        catch (IOException e)
        {
            throw new UnableToLoadException("Erreur lors du chargement des sessions", e);
        }

    }

    /// <summary>
    /// Sauvegarde les sessions des livres
    /// </summary>
    /// <param name="sessionsMap">(Dictionary<Isbn, Session>)</param>
    public void SaveBookSessions(Dictionary<Isbn, Session> sessionsMap) => SaveSessions(Mapper.SessionToDto(sessionsMap));

    /// <summary>
    /// Sauvegarde les sessions
    /// </summary>
    /// <param name="sessions">(List<Session>) liste des sessions</param>
    /// <exception cref="UnableToSaveException"></exception>
    private void SaveSessions(IReadOnlyList<SessionDTO> sessionsDto)
    {
        try
        {
            string jsonSessions = JsonConvert.SerializeObject(sessionsDto);

            File.WriteAllText(_JsonBooksPath, jsonSessions, Encoding.UTF8);
        }
        catch (Exception e)
        {
            throw new UnableToSaveException("Erreur lors de la sauvegarde des sessions", e);
        }

    }
}