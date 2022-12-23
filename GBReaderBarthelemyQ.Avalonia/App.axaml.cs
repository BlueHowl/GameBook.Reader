using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GBReaderBarthelemyQ.Avalonia.Pages;
using GBReaderBarthelemyQ.Domains;
using GBReaderBarthelemyQ.Domains.Exceptions;
using GBReaderBarthelemyQ.Presenters;
using GBReaderBarthelemyQ.Presenters.Interfaces.Notifications;
using GBReaderBarthelemyQ.Repository.Json;
using GBReaderBarthelemyQ.Repository.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace GBReaderBarthelemyQ.Avalonia
{
    public partial class App : Application
    {
        private const string DbConnectionString = @"server=192.168.132.200;port=13306;database=Q210043;uid=Q210043;pwd=0043;";

        private readonly string _jsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "ue36", "q210043-session.json");

        private MainWindow? _mainWindow;

        private SessionRepository? _sessionRepository;

        private Library? _library;

        public override void Initialize() => AvaloniaXamlLoader.Load(this);

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                _mainWindow = new MainWindow();
                desktop.MainWindow = _mainWindow;

                _mainWindow.Opened += MainWindow_Opened;
                _mainWindow.Closing += MainWindow_Closing;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void MainWindow_Opened(object? sender, EventArgs e) => Create();

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            try
            {
                Dictionary<Isbn, Session> sessionsMap = _library!.Books.ToDictionary(b => b.BookCover.Isbn, b => b.Session);
                _sessionRepository!.SaveBookSessions(sessionsMap);
            }
            catch (IsbnNotValidException ex)
            {
                _mainWindow!.Push(NotificationSeverity.Warning, "Erreur :", "Impossible de sauvergarder les sessions : " + ex.Message);
                //e.Cancel = true;
            }
        }

        /// <summary>
        /// Crée les differents elements du programmes
        /// </summary>
        private void Create()
        {
            SqlStorageFactory factory = new SqlStorageFactory("MySql.Data.MySqlClient", DbConnectionString);

            _sessionRepository = new SessionRepository(_jsonPath);

            _library = new Library(null!);


            var bookListView = new BookListView();
            var bookListPresenter = new BookListPresenter(bookListView, _mainWindow!, factory, _library, _sessionRepository);

            var pageDisplayView = new PageDisplayView();
            var pageDisplayPresenter = new PageDisplayPresenter(pageDisplayView, _mainWindow!, factory, _library);

            var statsView = new StatsView();
            var statsPresenter = new StatsPresenter(statsView, _mainWindow!, _library);

            _mainWindow!.RegisterPage("BookList", bookListView);
            _mainWindow!.RegisterPage("PageDisplay", pageDisplayView);
            _mainWindow!.RegisterPage("Stats", statsView);
        }
    }
}