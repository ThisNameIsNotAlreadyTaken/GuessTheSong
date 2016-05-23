using System.Windows;
using System.Windows.Controls;
using GuessTheSong.Models;
using GuessTheSong.ViewModels;

namespace GuessTheSong.Controls
{
    /// <summary>
    /// Interaction logic for SongButtonTabsControl.xaml
    /// </summary>
    public partial class SongButtonTabsControl
    {
        public SongButtonTabsControl()
        {
            InitializeComponent();
        }

        private static Song GetDataContext(object sender)
        {
            var menuItem = sender as MenuItem;
            return menuItem?.DataContext as Song; 
        }

        private void LockButton(object sender, RoutedEventArgs e)
        {
            var dataContext = GetDataContext(sender);
            if (dataContext != null) dataContext.IsGuessed = true;
        }

        private void UnlockButton(object sender, RoutedEventArgs e)
        {
            var dataContext = GetDataContext(sender);
            if (dataContext != null) dataContext.IsGuessed = false;
        }

        private void SkipButton(object sender, RoutedEventArgs e)
        {
            var dataContext = GetDataContext(sender);
            if (dataContext == null) return;

            var parent = Parent as Grid;
            var parentDataContext = parent?.DataContext as GameViewModel;

            parentDataContext?.SongSkip(dataContext);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dataContext = button?.DataContext as Song;
            if (dataContext == null) return;

            var parent = Parent as Grid;
            var parentDataContext = parent?.DataContext as GameViewModel;

            parentDataContext?.SongChoose(dataContext);
        }
    }
}
