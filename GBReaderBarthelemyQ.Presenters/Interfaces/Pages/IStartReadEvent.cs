using GBReaderBarthelemyQ.Presenters.Events;

namespace GBReaderBarthelemyQ.Presenters.Interfaces.Pages;

/// <summary>
/// Interface d'event StartReadEvent
/// </summary>
public interface IStartReadEvent
{
    event EventHandler<StartReadEventArgs> StartReadRequested;
}

