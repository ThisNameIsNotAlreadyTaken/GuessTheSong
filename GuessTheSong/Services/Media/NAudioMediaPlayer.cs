using System;
using NAudio.Wave;

namespace GuessTheSong.Services.Media
{
    /// <summary>
    /// NAudio Media Player implementation
    /// </summary>
    public class NaudioMediaPlayer : MediaPlayerBase
    {
        private IWavePlayer _wavePlayer;
        private AudioFileReader _audioFileReader;

        private bool _initialized;
        private Uri _source;

        public override TimeSpan Position
        {
            get { return _audioFileReader?.CurrentTime ?? TimeSpan.Zero; }
            set
            {
                if (_audioFileReader != null)
                {
                    _audioFileReader.CurrentTime = value;
                }
            }
        }

        public override TimeSpan Duration => _audioFileReader?.TotalTime ?? TimeSpan.Zero;

        public override Uri Source
        {
            get { return _source; }
            set
            {
                Stop();

                if (_audioFileReader != null)
                {
                    _audioFileReader.Dispose();
                    _audioFileReader = null;
                    _initialized = false;
                }

                _source = value;

                _audioFileReader = new AudioFileReader(_source.LocalPath);

                _wavePlayer.Init(_audioFileReader);

                _initialized = true;
            }
        }

        public override double Volume
        {
            get { return _audioFileReader?.Volume * 100 ?? 0.5*100;
            }
            set
            {
                if (_audioFileReader != null)
                {
                    _audioFileReader.Volume = (float)(value / 100);
                }
            }
        }

        public override void Initialize()
        {
            _wavePlayer = new WaveOut();
        }

        public override void Dispose()
        {
            _wavePlayer?.Stop();

            if (_audioFileReader != null)
            {
                _audioFileReader.Dispose();
                _audioFileReader = null;
            }

            if (_wavePlayer != null)
            {
                _wavePlayer.Dispose();
                _wavePlayer = null;
            }
        }

        public override void Play()
        {
            if (!_initialized && _audioFileReader != null)
            {
                _wavePlayer.Init(_audioFileReader);
                _initialized = true;
            }

            if (!_initialized)
                return;

            _wavePlayer.Play();
        }

        public override void Pause()
        {
            _wavePlayer.Pause();
        }

        public override void Stop()
        {
            _wavePlayer.Stop();
        }

        public override void AddMediaEndedHandler(EventHandler handler)
        {
            _wavePlayer.PlaybackStopped += new EventHandler<StoppedEventArgs>(handler);
        }
    }
}
