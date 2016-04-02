using System;
using System.Windows;
using System.Windows.Threading;

namespace GuessTheSong.Windows
{
    /// <summary>
    /// Interaction logic for SongInfoDialog.xaml
    /// </summary>
    public partial class SongInfoDialog
    {
        public SongInfoDialog()
        {
            InitializeComponent();
            StartCloseTimer();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void StartCloseTimer()
        {
            var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(5d)};
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            var timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= TimerTick;

            if (DialogResult != true)
            {
                DialogResult = true;
            }
        }
    }
}
