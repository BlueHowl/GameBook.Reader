using GBReaderBarthelemyQ.Domains;

namespace GBReaderBarthelemyQ.Repositories;

public interface ISessionRepository
{
    Dictionary<Isbn, Session> GetSessions();

    void SaveBookSessions(Dictionary<Isbn, Session> sessionsMap);
}

