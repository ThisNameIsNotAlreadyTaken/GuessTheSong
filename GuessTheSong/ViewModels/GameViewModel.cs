using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using GuessTheSong.Infrasctucture;
using GuessTheSong.Models;
using WpfExceptionViewer;
using GuessTheSong.Windows.Dialogs;

namespace GuessTheSong.ViewModels
{
    public class GameViewModel : ObservableObject
    {
        #region Properties

        private Song _selectedSong;

        public Song SelectedSong
        {
            get { return _selectedSong; }
            set
            {
                _selectedSong = value;
                NotifyPropertyChanged("SelectedSong");
                NotifyPropertyChanged("SelectedSongPreviewName");
            }
        }

        public string SelectedSongPreviewName => SelectedSong != null ? $"Playing: {SelectedSong.CategoryName} - {SelectedSong.Price}" : null;

        public ObservableCollection<GameParticipant> Participants { get; set; }

        public GameData GameData { get; set; }

        public PlayerViewModel PlViewModel { get; set; } = new PlayerViewModel();

        private bool _showRemovePointsButtons;

        public bool ShowRemovePointsButtons
        {
            get { return _showRemovePointsButtons; }
            set
            {
                _showRemovePointsButtons = value;
                NotifyPropertyChanged("ShowRemovePointsButtons");
            }
        }

        #endregion

        public GameViewModel(GameData gameData, List<GameParticipant> participants)
        {
            Participants = new ObservableCollection<GameParticipant>(participants);
            GameData = gameData; 
        }

        ~GameViewModel()
        {
            Participants = null;
            GameData = null;
        }

        #region Commands

        public void PriceWinner(GameParticipant participant)
        {
            participant.Score += SelectedSong.Price;

            if (!SelectedSong.IsGuessed)
            {
                new SongInfoDialog {DataContext = SelectedSong}.ShowDialog();
            }

            SelectedSong.IsGuessed = true;
        }

        public ICommand PriceWinnerCommand => new DelegateParametrizedCommand<GameParticipant>(PriceWinner);

        public void PunishWinner(GameParticipant participant)
        {
            participant.Score -= SelectedSong.Price;
        }

        public ICommand PunishWinnerCommand => new DelegateParametrizedCommand<GameParticipant>(PunishWinner);

        private void CloseException()
        {
            PlViewModel.PlayerException = null;
        }

        public ICommand CloseExceptionCommand => new DelegateCommand(CloseException);


        private void ShowException()
        {
            new ExceptionViewer("An unexpected error occurred in the application.", PlViewModel.PlayerException).ShowDialog();
        }

        public ICommand ShowExceptionCommand => new DelegateCommand(ShowException);

        #endregion

        public void SongChoose(Song song)
        {
            if (song == null) return;

            if (SelectedSong != null)
            {
                SelectedSong.IsSelected = false;
            }

            SelectedSong = song;
            SelectedSong.IsSelected = true;
            SelectedSong.IsDelayed = false;

            if (!File.Exists(SelectedSong.File.FullPath)) return;

            if (SelectedSong.IsPigInTheBox)
            {
                if (PlViewModel.IsPlaying)
                {
                    PlViewModel.Pause();
                }

                new PigInTheBoxDialog().ShowDialog();
            }

            PlViewModel.CurrentAudio = SelectedSong;
        }

        public void SongSkip(Song song)
        {
            if (song == null) return;

            song.IsDelayed = true;

            if (PlViewModel.IsPlaying) PlViewModel.Pause();
        }
    }
}
