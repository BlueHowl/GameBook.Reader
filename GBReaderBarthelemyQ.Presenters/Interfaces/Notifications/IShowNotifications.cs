namespace GBReaderBarthelemyQ.Presenters.Interfaces.Notifications;

/// <summary>
/// Interface d'envoi de notifications
/// </summary>
public interface IShowNotifications
{
    void Push(NotificationSeverity serverity, string title, string message);
}
