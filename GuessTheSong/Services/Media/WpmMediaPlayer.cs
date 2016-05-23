using System;
using System.Windows.Media;

namespace GuessTheSong.Services.Media
{
    /// <summary>
    /// Wrapper on MediaElement which uses Windows Media Player engine
    /// </summary>
    public class WmpMediaPlayer : MediaPlayerBase
    {
        private MediaPlayer _mediaPlayer;

        public override TimeSpan Position
        {
            get { return _mediaPlayer.Position; }
            set { _mediaPlayer.Position = value; }
        }

        public override TimeSpan Duration
        {
            get
            {
                if (_mediaPlayer.NaturalDuration.HasTimeSpan)
                    return _mediaPlayer.NaturalDuration.TimeSpan;

                return TimeSpan.Zero;
            }
        }

        public override Uri Source
        {
            get { return _mediaPlayer.Source; }
            set { _mediaPlayer.Open(value); }
        }

        public override double Volume
        {
            get { return _mediaPlayer.Volume*100; }
            set { _mediaPlayer.Volume = value/100; }
        }

        public override void Initialize()
        {
            _mediaPlayer = new MediaPlayer();
        }

        public override void Play()
        {
            _mediaPlayer.Play();
        }

        public override void Pause()
        {
            _mediaPlayer.Pause();
        }

        public override void Stop()
        {
            _mediaPlayer.Stop();
        }

        public override void Dispose()
        {
            _mediaPlayer = null;
        }

        public override void AddMediaEndedHandler(EventHandler handler)
        {
            _mediaPlayer.MediaEnded += handler;
        }
    }
}
