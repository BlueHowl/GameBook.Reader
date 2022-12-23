using GBReaderBarthelemyQ.Presenters.Events;

namespace GBReaderBarthelemyQ.Presenters.Interfaces.Routes;

/// <summary>
/// Interface routeur
/// </summary>
public interface IBrowseToViews
{
    void GoTo(string view);

    event EventHandler<RestartReadEventArgs> StartReadRequested;

    public event EventHandler StatsRequested;
}
