using System.Windows;

namespace GuessTheSong.Windows
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

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
