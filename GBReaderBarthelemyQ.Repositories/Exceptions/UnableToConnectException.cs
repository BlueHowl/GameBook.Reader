namespace GBReaderBarthelemyQ.Repositories.Exceptions;

public class UnableToConnectException : Exception
{
    public UnableToConnectException(string message, Exception ex)
        : base(message, ex)
    { }
}