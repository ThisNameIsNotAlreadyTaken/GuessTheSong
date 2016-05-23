using System.Windows;

namespace GuessTheSong.Windows.Dialogs
{
    /// <summary>
    /// Interaction logic for AddParticipantDialog.xaml
    /// </summary>
    public partial class AddParticipantDialog
    {
        public AddParticipantDialog()
        {
            InitializeComponent();
        }

        public string ResponseText
        {
            get { return ResponseTextBox.Text; }
            set { ResponseTextBox.Text = value; }
        }

        private void OkButton(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
