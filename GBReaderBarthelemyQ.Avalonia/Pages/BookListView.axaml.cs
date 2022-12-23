using Avalonia.Controls;
using Avalonia.Interactivity;
using GBReaderBarthelemyQ.Avalonia.Items;
using GBReaderBarthelemyQ.Domains;
using GBReaderBarthelemyQ.Presenters.Events;
using GBReaderBarthelemyQ.Presenters.Interfaces.Pages;
using System;
using System.Collections.Generic;

namespace GBReaderBarthelemyQ.Avalonia.Pages
{
    /// <summary>
    /// Page liste des livres
    /// </summary>
    public partial class BookListView : UserControl, IBookListView
    {
        /// <summary>
        /// Constructeur de la Page
        /// </summary>
        public BookListView() => InitializeComponent();

        /// <summary>
        /// Affiche la liste de livre à afficher
        /// </summary>
        /// <param name="books">(List<Book>)</param>
        public void DisplayBooks(List<Book> books)
        {
            if (books.Count == 0)
            {
                BVPanelMessage.IsVisible = true;
            }
            else
            {
                foreach (var book in books)
                {
                    AddBook(book.BookCover, book.Session != null);
                }

                BVPanelMessage.IsVisible = false;
            }
        }

        /// <summary>
        /// Ajoute un livre à afficher au containeur BookViewStackPanel
        /// </summary>
        /// <param name="cover">(BookModel) livre à afficher</param>
        private void AddBook(Cover cover, bool hasSession)
        {
            var bookView = new BookView();
            bookView.SetInformations(cover, hasSession);//tochange

            BookViewStackPanel.Children.Add(bookView);
            bookView.StartReadRequested += this.OnStartReadRequested;

            if (BookViewStackPanel.Children.Count == 1)
            {
                bookView.Get<Expander>("BVExpander").IsExpanded = true;
            }
        }

        /// <summary>
        /// Est appelé lors de l'appui sur le boutton filtre, cherche et affiche les livres correspondants à la recherche
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Filter(object sender, RoutedEventArgs e)
        {
            if (ResearchBox.Text != null)
            {
                BookViewStackPanel.Children.Clear();
                FilterRequested?.Invoke(this, new FilterEventArgs(ResearchBox.Text.Trim()));
            }
        }

        /// <summary>
        /// Est appelé lors de l'appui sur le boutton stats affiche la Page des stats
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoToStats(object sender, RoutedEventArgs e) => StatsRequested?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Est appelé lors de l'appui sur un boutton de lecture d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartReadRequested(object? sender, StartReadEventArgs e) => StartReadRequested?.Invoke(this, new StartReadEventArgs(e.BookIsbn));

        public event EventHandler<FilterEventArgs>? FilterRequested;

        public event EventHandler<StartReadEventArgs>? StartReadRequested;

        public event EventHandler? StatsRequested;
    }
}
