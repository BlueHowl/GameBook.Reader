namespace GBReaderBarthelemyQ.Repositories.Exceptions;

public class UnableToSaveException : Exception
{
    public UnableToSaveException(string message, Exception ex)
        : base(message, ex)
    { }
}
