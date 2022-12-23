namespace GBReaderBarthelemyQ.Repositories.Exceptions;

public class UnableToLoadException : Exception
{
    public UnableToLoadException(string message, Exception ex)
        : base(message, ex)
    { }
}