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

        private readonly DispatcherTimer _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };

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

        private void TimerEventHandler(object sender, EventArgs e)
        {
            NotifyPropertyChanged("TimerTick");
            NotifyPropertyChanged("PlayerPosition");
        }

        public string TimerTick
        {
            get
            {
                if (_mediaPlayer.Source == null || !_mediaPlayer.NaturalDuration.HasTimeSpan) return "No file selected...";
                return string.Format("{0} / {1}", _mediaPlayer.Position.ToString(@"mm\:ss"), _mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
        }

        public double PlayerPosition
        {
            get
            {
                if (_mediaPlayer.Source == null || !_mediaPlayer.NaturalDuration.HasTimeSpan) return 0d;

                return (_mediaPlayer.Position.TotalSeconds/ _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds)*100;
            }
            set
            {
                _mediaPlayer.Position = TimeSpan.FromSeconds(_mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds*value/100);
            }
        } 

        public GameViewModel(List<Category> gameData, List<GameParticipant> participants)
        {
            Participants = new ObservableCollection<GameParticipant>(participants);
            GameData = new ObservableCollection<Category>(gameData);
            _timer.Tick += TimerEventHandler;
        }

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

            _mediaPlayer.Open(new Uri(SelectedSong.File.FullPath));
            Play();
        }

        public void PriceWinner(GameParticipant participant)
        {
            Pause();
            participant.Score += SelectedSong.Price;
            SelectedSong.IsGuessed = true;
        }

        public void PunishWinner(GameParticipant participant)
        {
            Pause();
            participant.Score -= SelectedSong.Price;
        }

        private void Play()
        {
            _mediaPlayer.Play();
            _timer.Start();
        }

        public ICommand PlayCommand => new DelegateCommand(Play);

        private void Pause()
        {
            _mediaPlayer.Pause();
            _timer.Stop();
        }

        public ICommand PauseCommand => new DelegateCommand(Pause);

        private void Stop()
        {
            _mediaPlayer.Stop();
            _timer.Stop();
        }

        public ICommand StopCommand => new DelegateCommand(Stop);
    }
}
