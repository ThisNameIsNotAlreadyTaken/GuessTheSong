using System;

namespace GuessTheSong.Services.Media
{
    public enum MediaEngine
    {
        /// <summary>
        /// Windows Media Player engine
        /// </summary>
        Wmp,
        /// <summary>
        /// NAudio engine
        /// </summary>
        NAudio
    }

    public abstract class MediaPlayerBase : IDisposable
    {
        //fields
        public abstract TimeSpan Position { get; set; }
        public abstract TimeSpan Duration { get; }
        public abstract Uri Source { get; set; }
        public abstract double Volume { get; set; }

        //events
        public EventHandler MediaOpened;
        public EventHandler<Exception> MediaFailed;

        //methods
        public abstract void Initialize();
        public abstract void Play();
        public abstract void Pause();
        public abstract void Stop();

        public abstract void Dispose();

        public abstract void AddMediaEndedHandler(EventHandler handler);
    }
}
