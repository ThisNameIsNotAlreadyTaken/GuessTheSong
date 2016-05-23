using System.Windows;
using GuessTheSong.Models;
using GuessTheSong.Windows.Dialogs;

namespace GuessTheSong.Controls
{
    /// <summary>
    /// Interaction logic for ParticipantsList.xaml
    /// </summary>
    public partial class ParticipantsList
    {
        public ParticipantsList()
        {
            InitializeComponent();
        }

        private GameParticipant GetSelectedParticipant()
        {
            if (ParticipantsListBox.SelectedItems.Count == 0) return null;
            return ParticipantsListBox.SelectedItems[0] as GameParticipant;
        }

        private void AddPoints(object sender, RoutedEventArgs e)
        {
            var p = GetSelectedParticipant();
            if (p != null) new PointsChangeDialog(p, PointsChangeDialogType.Add).ShowDialog();
        }

        private void RemovePoints(object sender, RoutedEventArgs e)
        {
            var p = GetSelectedParticipant();
            if (p != null) new PointsChangeDialog(p, PointsChangeDialogType.Remove).ShowDialog();
        }

        private void SetPoints(object sender, RoutedEventArgs e)
        {
            var p = GetSelectedParticipant();
            if (p != null) new PointsChangeDialog(p, PointsChangeDialogType.Set).ShowDialog();
        }
    }
}
