using Avalonia.Controls;

namespace GBReaderBarthelemyQ.Avalonia.Items
{
    public partial class SessionView : UserControl
    {
        public SessionView()
        {
            InitializeComponent();
        }

        internal void SetInformations(string bookInfos, string startDate, string lastDate)
        {
            BookInfos.Text = bookInfos;
            StartDate.Text = startDate;
            LastDate.Text = lastDate;
        }
    }
}
