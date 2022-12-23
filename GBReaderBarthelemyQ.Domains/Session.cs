using GBReaderBarthelemyQ.Domains.Exceptions;

namespace GBReaderBarthelemyQ.Domains;

/// <summary>
/// Classe session
/// </summary>
public class Session
{
    private DateTime _start;

    private DateTime _last;

    private int _lastPageNum;

    public DateTime Start
    {
        get => _start;
        set => _start = value;
    }

    public DateTime Last
    {
        get => _last;
        set => _last = value;
    }

    public int LastPageNum
    {
        get => _lastPageNum;
        set => _lastPageNum = value;
    }

    public Session()
    {
        Start = DateTime.UtcNow;
        LastPageNum = 0;
    }

    public Session(DateTime start, DateTime last, int pageNum)
    {
        CheckPageNumber(pageNum);

        Start = start;
        Last = last;
        LastPageNum = pageNum;
    }

    public void UpdateLastState(int pageNum)
    {
        CheckPageNumber(pageNum);

        LastPageNum = pageNum;
        Last = DateTime.UtcNow;
    }

    private void CheckPageNumber(int pageNum)
    {
        if (pageNum < 0)
        {
            throw new SessionNotValidException("Le numéro de page sauvegardé en session est invalide");
        }
    }
}

