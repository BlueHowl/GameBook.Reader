namespace GBReaderBarthelemyQ.Domains.Exceptions;

public class SessionNotValidException : Exception
{
    public SessionNotValidException(string message)
        : base(message)
    { }
}

