using Avalonia.Controls;
using Avalonia.Interactivity;
using GBReaderBarthelemyQ.Avalonia.Items;
using GBReaderBarthelemyQ.Domains;
using GBReaderBarthelemyQ.Presenters.Interfaces.Pages;
using System;
using System.Collections.Generic;

namespace GBReaderBarthelemyQ.Avalonia.Pages
{
    public partial class StatsView : UserControl, IStatsView
    {
        public StatsView() => InitializeComponent();

        /// <summary>
        /// Affiche la liste de livre � afficher
        /// </summary>
        /// <param name="books">(List<Book>)</param>
        public void DisplaySession(List<Book> books)
        {

            if (books.Count == 0)
            {
                SVPanelMessage.IsVisible = true;
            }
            else
            {
                SessionViewStackPanel.Children.Clear();

                int sessionCount = 0;

                foreach (var book in books)
                {
                    if (book.Session != null)
                    {
                        string bookInfos = book.BookCover.Title + " - " + book.BookCover.Isbn;
                        AddSession(bookInfos, "Date de d�but : " + book.Session.Start.ToString(), "Dernier choix : " + book.Session.Last.ToString());

                        ++sessionCount;
                    }
                }

                SessionCount.Text = "Nombre de sessions : " + sessionCount;

                SVPanelMessage.IsVisible = false;
            }
        }

        /// <summary>
        /// Ajoute une session � afficher au containeur SessionViewStackPanel
        /// </summary>
        /// <param name="cover">(BookModel) livre � afficher</param>
        private void AddSession(string bookInfos, string startDate, string lastDate)
        {
            SessionView sessionView = new SessionView();
            sessionView.SetInformations(bookInfos, startDate, lastDate);
            SessionViewStackPanel.Children.Add(sessionView);
        }

        /// <summary>
        /// Est appel� lors de l'appui sur le boutton retour, affiche la page menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back(object sender, RoutedEventArgs e) => BackRequested?.Invoke(this, EventArgs.Empty);

        public event EventHandler? BackRequested;
    }
}
