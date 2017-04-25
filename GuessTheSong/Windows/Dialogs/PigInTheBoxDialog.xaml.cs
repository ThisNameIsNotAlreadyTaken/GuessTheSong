using System.Windows;

namespace GuessTheSong.Windows.Dialogs
{
    /// <summary>
    /// Interaction logic for PigInTheBoxDialog.xaml
    /// </summary>
    public partial class PigInTheBoxDialog
    {
        public PigInTheBoxDialog()
        {
            InitializeComponent();
        }

        private void OkButton(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}