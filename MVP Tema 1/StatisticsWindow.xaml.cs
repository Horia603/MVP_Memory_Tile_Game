using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MVP_Tema_1
{
    /// <summary>
    /// Interaction logic for StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        private User currentPlayer;
        public StatisticsWindow(User user)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            currentPlayer = user;
            PlayerName.Text = currentPlayer.UserName;
            string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "Resource\\ProfilePhotos\\" + currentPlayer.Photo));
            PlayerImage.Source = new BitmapImage(new Uri(filePath, UriKind.Absolute));
            PlayedGames.Text = currentPlayer.PlayedGames.ToString();
            WinnedGames.Text = currentPlayer.WinnedGames.ToString();
            Closing += this.OnWindowClosing;
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            MainWindow window = new MainWindow(currentPlayer);
            window.Show();
        }
    }
}
