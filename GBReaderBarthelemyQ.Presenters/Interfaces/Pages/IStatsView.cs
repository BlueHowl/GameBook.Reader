using GBReaderBarthelemyQ.Domains;

namespace GBReaderBarthelemyQ.Presenters.Interfaces.Pages;

public interface IStatsView
{
    void DisplaySession(List<Book> books);

    event EventHandler BackRequested;
}