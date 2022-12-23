using GBReaderBarthelemyQ.Domains;

namespace GBReaderBarthelemyQ.Presenters.Events;

/// <summary>
/// Argument d'event de lecture de livre
/// </summary>
/// <param name="BookIsbn">(Isbn)</param>
public record StartReadEventArgs(Isbn BookIsbn);
