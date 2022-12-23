namespace GBReaderBarthelemyQ.Presenters.Events;

/// <summary>
/// Argument d'event de lecture de livre
/// </summary>
/// <param name="restart">(Boolean)</param>
public record RestartReadEventArgs(Boolean restart);