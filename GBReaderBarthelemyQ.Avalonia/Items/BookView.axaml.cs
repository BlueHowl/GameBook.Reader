using Avalonia.Controls;
using Avalonia.Interactivity;
using GBReaderBarthelemyQ.Domains;
using GBReaderBarthelemyQ.Presenters.Events;
using GBReaderBarthelemyQ.Presenters.Interfaces.Pages;
using System;

namespace GBReaderBarthelemyQ.Avalonia.Items
{
    public partial class BookView : UserControl, IStartReadEvent
    {
        private Cover? _cover;
        private bool _hasSession;

        public BookView()
        {
            InitializeComponent();
        }

        public void SetInformations(Cover cover, bool hasSession)
        {
            _cover = cover;
            _hasSession = hasSession;

            MapInformations();
        }

        /// <summary>
        /// Map les informations du livre aux champs de l'userControl
        /// </summary>
        private void MapInformations()
        {
            TitleText.Text = _cover!.Title;
            SummaryText.Text = _cover.Summary;
            AuthorText.Text = _cover.Author;
            IsbnText.Text = _cover.Isbn.ToString();

            ChangeBtnContent();
        }

        private void ChangeBtnContent() => StartReadBtn.Content = _hasSession ? "Continuer la lecture" : "Démarrer la lecture";

        /// <summary>
        /// Est appelé lors d'un click sur le bouton StartRead
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartRead(object sender, RoutedEventArgs e)
        {
            StartReadRequested?.Invoke(this, new StartReadEventArgs(_cover!.Isbn));

            _hasSession = true;
            ChangeBtnContent();
        }

        public event EventHandler<StartReadEventArgs>? StartReadRequested;
    }

}
