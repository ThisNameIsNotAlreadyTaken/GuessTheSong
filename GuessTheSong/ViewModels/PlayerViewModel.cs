using System;
using System.Windows.Input;
using System.Windows.Threading;
using GuessTheSong.Infrasctucture;
using GuessTheSong.Models;
using GuessTheSong.Services.Media;
using GuessTheSong.Windows.Dialogs;

namespace GuessTheSong.ViewModels
{
    public class PlayerViewModel : ObservableObject
    {
        #region PlayerProperties

        private MediaPlayerBase _wmpMediaPlayer;
        private MediaPlayerBase _naudioMediaPlayer;

        private MediaEngine _currentMediaEngine;

        public MediaEngine CurrentMediaEngine
        {
            get { return _currentMediaEngine; }
            set
            {
                if (_currentMediaEngine == value && MediaPlayer != null) return;

                if (IsPlaying) Stop();

                _currentMediaEngine = value;

                var player = GetPlayerByMediaEngine(_currentMediaEngine);

                if (CurrentAudio != null)
                {
                    var currentUri = new Uri(CurrentAudio.File.FullPath);
                         
                    if (player.Source == null || player.Source.AbsolutePath != currentUri.AbsolutePath)
                    {
                        try
                        {
                            player.Source = currentUri;
                            ToBeginning();
                        }
                        catch (Exception e)
                        {
                            PlayerException = e;
                        }

                    }
                }

                MediaPlayer = player;
                PositionTimerTick(null, null);
            }
        }

        private MediaPlayerBase MediaPlayer { get; set; }

        private MediaPlayerBase GetPlayerByMediaEngine(MediaEngine engine)
        {
            MediaPlayerBase result;

            switch (engine)
            {
                case MediaEngine.Wmp:
                    if (_wmpMediaPlayer == null)
                    {
                        _wmpMediaPlayer = new WmpMediaPlayer();
                        _wmpMediaPlayer.Initialize();
                        _wmpMediaPlayer.AddMediaEndedHandler(OnMediaEnded);
                    }
                    result = _wmpMediaPlayer;
                    break;
                case MediaEngine.NAudio:
                    if (_naudioMediaPlayer == null)
                    {
                        _naudioMediaPlayer = new NaudioMediaPlayer();
                        _naudioMediaPlayer.Initialize();
                        _naudioMediaPlayer.AddMediaEndedHandler(OnMediaEnded);
                    }
                    result = _naudioMediaPlayer;
                    break;
                default:
                    return null;
            }

            return result;
        }

        private Exception _playerException;

        public Exception PlayerException
        {
            get { return _playerException; }
            set
            {
                _playerException = value;
                NotifyPropertyChanged("PlayerException");
            }
        }

        #endregion

        #region Position&VolumeProperties

        private readonly DispatcherTimer _positionTimer;

        public TimeSpan CurrentAudioPosition
        {
            get { return MediaPlayer?.Position ?? TimeSpan.Zero; }
            set
            {
                if (MediaPlayer == null)
                    return;

                if (Math.Abs(MediaPlayer.Position.TotalSeconds - value.TotalSeconds) < 0.0001)
                    return;

                MediaPlayer.Position = value;
            }
        }

        public double CurrentAudioPositionSeconds
        {
            get { return CurrentAudioPosition.TotalSeconds; }
            set { CurrentAudioPosition = TimeSpan.FromSeconds(value); }
        }

        public TimeSpan CurrentAudioDuration => MediaPlayer?.Duration ?? TimeSpan.Zero;

        public double Volume
        {
            get { return MediaPlayer.Volume; }
            set
            {
                MediaPlayer.Volume = value;
                NotifyPropertyChanged("Volume");
            }
        }

        private void PositionTimerTick(object sender, object e)
        {
            NotifyPropertyChanged("CurrentAudioPosition");
            NotifyPropertyChanged("CurrentAudioDuration");
            NotifyPropertyChanged("CurrentAudioPositionSeconds");
        }

        #endregion

        public PlayerViewModel()
        {
            CurrentMediaEngine = MediaEngine.Wmp;

            _positionTimer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(500)};
            _positionTimer.Tick += PositionTimerTick;
        }

        ~PlayerViewModel()
        {
            _positionTimer.Stop();
            _wmpMediaPlayer?.Dispose();
            _naudioMediaPlayer?.Dispose();
        }

        private Song _currentAudio;

        public Song CurrentAudio
        {
            get { return _currentAudio; }
            set
            {
                if (_currentAudio == value) return;

                _currentAudio = value;

                NotifyPropertyChanged("CurrentAudio");

                Play(true);
            }
        }

        private bool _isPlaying;

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                _isPlaying = value;
                NotifyPropertyChanged("IsPlaying");
            }
        }

        #region Commands

        private void PlayPause()
        {
            if (IsPlaying) Pause();
            else Play();
        }

        public ICommand PlayPauseCommand => new DelegateCommand(PlayPause);

        public void Play(bool audioChanged = false)
        {
            if ((MediaPlayer.Source == null && CurrentAudio != null) || (audioChanged && CurrentAudio != null))
            {
                try
                {
                    MediaPlayer.Source = new Uri(CurrentAudio.File.FullPath);
                }
                catch (Exception e)
                {
                    PlayerException = e;
                    return;
                }
            }

            try
            {
                MediaPlayer.Play();
                _positionTimer.Start();
                IsPlaying = true;
            }
            catch (Exception e)
            {
                PlayerException = e;
            }
        }

        public void Pause()
        {
            MediaPlayer.Pause();
            _positionTimer.Stop();
            IsPlaying = false;
        }

        public ICommand PauseCommand => new DelegateCommand(Pause);

        public void Stop()
        {
            MediaPlayer.Stop();
            _positionTimer.Stop();
            IsPlaying = false;
        }

        public ICommand StopCommand => new DelegateCommand(Stop);

        private void ToBeginning()
        {
            CurrentAudioPosition = TimeSpan.Zero;
            PositionTimerTick(null, null);
        }

        public ICommand ToBeginningCommand => new DelegateCommand(ToBeginning);

        public void ToTheEnd()
        {
            CurrentAudioPosition = TimeSpan.FromSeconds(CurrentAudioDuration.TotalSeconds * 0.9);
            PositionTimerTick(null, null);
        }

        public ICommand ToTheEndCommand => new DelegateCommand(ToTheEnd);

        private void ShowSongInfo()
        {
            new SongInfoDialog { DataContext = CurrentAudio }.ShowDialog();
        }

        public ICommand ShowSongInfoCommand => new DelegateCommand(ShowSongInfo);

        #endregion

        private void OnMediaEnded(object sender, EventArgs e)
        {
            MediaPlayer.Stop();
            ToBeginning();
        }
    }
}
