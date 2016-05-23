using System.Collections.Generic;
using System.Windows;

namespace GuessTheSong.Windows.Dialogs
{
    /// <summary>
    /// Interaction logic for WarningsDialog.xaml
    /// </summary>
    public partial class WarningsDialog
    {
        public WarningsDialog(List<string> warnings)
        {
            InitializeComponent();
            DataContext = warnings;
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
