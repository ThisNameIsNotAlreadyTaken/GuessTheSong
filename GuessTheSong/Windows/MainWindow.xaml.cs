using GuessTheSong.ViewModels;

namespace GuessTheSong.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();
        } 
    }
}