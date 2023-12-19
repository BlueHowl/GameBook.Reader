using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using GBReaderBarthelemyQ.Presenters.Events;
using GBReaderBarthelemyQ.Presenters.Interfaces;
using GBReaderBarthelemyQ.Presenters.Interfaces.Notifications;
using System;
using System.Collections.Generic;

namespace GBReaderBarthelemyQ.Avalonia
{
    /// <summary>
    /// Classe MainWindow (vue)
    /// </summary>
    public partial class MainWindow : Window, INotificationAndRoute
    {
        private readonly WindowNotificationManager _notificationManager;

        private readonly IDictionary<string, UserControl> _pages = new Dictionary<string, UserControl>();

        /// <summary>
        /// Constructeur de la vue
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _notificationManager = new WindowNotificationManager(this)
            {
                Position = NotificationPosition.BottomRight
            };
        }

        internal void RegisterPage(string pageName, UserControl page)
        {
            _pages[pageName] = page;
            Content ??= page;
        }

        public void GoTo(string pageName)
        {
            Content = _pages[pageName];

            switch (pageName)    ///PRBLM OCP
            {
                case "PageDisplay":
                    StartReadRequested?.Invoke(this, new RestartReadEventArgs(false));
                    break;
                case "Stats":
                    StatsRequested?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        public void Push(NotificationSeverity severity, string title, string message)
        {
            var notification = new Notification(title, message, severity switch
            {
                NotificationSeverity.Info => NotificationType.Information,
                NotificationSeverity.Warning => NotificationType.Warning,
                NotificationSeverity.Success => NotificationType.Success,
                _ => NotificationType.Error,
            });

            if (this.IsVisible)
            {
                _notificationManager.Show(notification);
            }
        }

        public event EventHandler<RestartReadEventArgs>? StartReadRequested;

        public event EventHandler? StatsRequested;
    }
}