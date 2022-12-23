namespace GBReaderBarthelemyQ.Domains.Exceptions;

public class IsbnNotValidException : Exception
{
    public IsbnNotValidException(string message)
        : base(message)
    { }
}

