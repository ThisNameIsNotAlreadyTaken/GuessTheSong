using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using GuessTheSong.Helpers;
using GuessTheSong.Models;

namespace GuessTheSong.Windows.Dialogs
{
    public enum PointsChangeDialogType
    {
        Add,
        Remove,
        Set
    }

    /// <summary>
    /// Interaction logic for PointsChangeDialog.xaml
    /// </summary>
    public partial class PointsChangeDialog
    {
        private readonly GameParticipant _participant;
        private readonly Action<int> _resultDelegate;

        public PointsChangeDialog(GameParticipant participant, PointsChangeDialogType type)
        {
            InitializeComponent();
            _participant = participant;
            Title = $"{type} {participant.Name}'s points";
            switch (type)
            {
                case PointsChangeDialogType.Add:
                    _resultDelegate = AddPoints;
                    break;
                case PointsChangeDialogType.Remove:
                    _resultDelegate = RemovePoints;
                    break;
                case PointsChangeDialogType.Set:
                    _resultDelegate = SetPoints;
                    break;
            }
        }

        public void AddPoints(int points) => _participant.Score += points;
        public void RemovePoints(int points) => _participant.Score -= points;
        public void SetPoints(int points) => _participant.Score = points;

        private void OkButton(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_resultDelegate != null && !ResponseTextBox.Text.IsNullOrEmpty())
            {
                int points;

                if (int.TryParse(ResponseTextBox.Text, out points))
                {
                    _resultDelegate.Invoke(points);
                }
            }

            DialogResult = true;
        }

        private void CancelButton(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
