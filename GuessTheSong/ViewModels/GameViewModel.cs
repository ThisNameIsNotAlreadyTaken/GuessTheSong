using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using GuessTheSong.Infrasctucture;
using GuessTheSong.Models;

namespace GuessTheSong.ViewModels
{
    //TODO: add save for later button and colorize that option
    public class GameViewModel : ObservableObject
    {
        public ObservableCollection<GameParticipant> Participants { get; set; }

        public ObservableCollection<Category> GameData { get; set; }

        private readonly MediaPlayer _mediaPlayer = new MediaPlayer();

        private Song _selectedSong;

        private void TimerEventHandler(object sender, EventArgs e)
        {
            NotifyPropertyChanged("TimerTick");
        }

        public string TimerTick
        {
            get
            {
                if (_mediaPlayer.Source != null && _mediaPlayer.NaturalDuration.HasTimeSpan)
                    return string.Format("{0} / {1}", _mediaPlayer.Position.ToString(@"mm\:ss"), 
                        _mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                return "No file selected...";
            }
        }

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

        public string SelectedSongPreviewName
        {
            get
            {
                if (SelectedSong != null)
                {
                    return string.Format("Playing: {0} - {1}", GameData.First(x => x.Songs.Contains(SelectedSong)).Name,
                        SelectedSong.Price);
                }
                return null;
            }
        }

        public GameViewModel(List<Category> gameData, List<GameParticipant> participants)
        {
            Participants = new ObservableCollection<GameParticipant>(participants);
            GameData = new ObservableCollection<Category>(gameData);

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += TimerEventHandler;
            timer.Start();
        }

        public void SongChoose(Song song)
        {
            if (song == null) return;
            SelectedSong = song;

            if (!File.Exists(SelectedSong.File.FullPath)) return;
            _mediaPlayer.Open(new Uri(SelectedSong.File.FullPath));
            Play();
        }

        public void PriceWinner(GameParticipant participant)
        {
            Pause();
            participant.Score += SelectedSong.Price;
            SelectedSong.IsGuessed = true;
        }

        private void Play()
        {
            _mediaPlayer.Play();
        }

        public ICommand PlayCommand => new DelegateCommand(Play);

        private void Pause()
        {
            _mediaPlayer.Pause();
        }

        public ICommand PauseCommand => new DelegateCommand(Pause);

        private void Stop()
        {
            _mediaPlayer.Stop();
        }

        public ICommand StopCommand => new DelegateCommand(Stop);
    }
}
