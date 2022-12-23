using Avalonia.Controls;
using Avalonia.Interactivity;
using GBReaderBarthelemyQ.Presenters.Events;
using GBReaderBarthelemyQ.Presenters.Interfaces.Pages;
using GBReaderBarthelemyQ.Presenters.ViewModels;
using System;

namespace GBReaderBarthelemyQ.Avalonia.Pages;

/// <summary>
/// Page d'affichage de pages
/// </summary>
public partial class PageDisplayView : UserControl, IPageDisplayView
{

    private PageViewModel _pageViewModel;

    /// <summary>
    /// Constructeur de la page
    /// </summary>
    public PageDisplayView() => InitializeComponent();

    /// <summary>
    /// Affiche la page donnée
    /// </summary>
    /// <param name="pageViewModel">(PageViewModel) modèle de vue d'une page</param>
    public void DisplayPage(PageViewModel pageViewModel)
    {
        _pageViewModel = pageViewModel;
        PageNumber.Text = "Page N°" + pageViewModel.PageNumber.ToString();
        PageText.Text = pageViewModel.Text;

        TurnToEndMenu(pageViewModel.Choices.Count > 0);
        ChangeChoiceList();
    }

    /// <summary>
    /// Rends le menu de fin visible ou non
    /// </summary>
    /// <param name="b"></param>
    private void TurnToEndMenu(bool b)
    {
        ChoiceMenu.IsVisible = b;
        EndMenu.IsVisible = !b;
    }

    /// <summary>
    /// Met à jour la listbox des choix
    /// </summary>
    private void ChangeChoiceList()
    {
        ChoiceList.SelectionChanged -= ListBoxSelectionChanged!;

        ChoiceList.SelectedIndex = -1;
        ChoiceList.Items = _pageViewModel.ChoicesText;

        ChoiceList.SelectionChanged += ListBoxSelectionChanged!;
    }

    /// <summary>
    /// Est appelé lors d'un click sur le bouton GoToBookListPage
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GoToBookListPage(object sender, RoutedEventArgs e) => GoToBookListRequested?.Invoke(this, EventArgs.Empty);

    /// <summary>
    /// Est appelé lors d'un click sur le bouton RestartRead
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RestartRead(object sender, RoutedEventArgs e) => StartReadRequested?.Invoke(this, new RestartReadEventArgs(true));


    /// <summary>
    /// Est appelé lors de la sélection d'un objet dans la listbox
    /// </summary>
    /// <param name="sender">Objet</param>
    /// <param name="e">Arguments</param>
    private void ListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        int index = _pageViewModel.ChoicesText.IndexOf(e.AddedItems[0]!.ToString()!);

        NextPageRequested?.Invoke(this, new NextPageEventArgs(_pageViewModel.Choices[index].PageRef));
    }

    public event EventHandler<NextPageEventArgs>? NextPageRequested;

    public event EventHandler? GoToBookListRequested;

    public event EventHandler<RestartReadEventArgs>? StartReadRequested;
}