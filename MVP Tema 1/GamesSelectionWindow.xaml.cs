using System.Windows;
using System.Windows.Controls;

namespace MVP_Tema_1
{
    /// <summary>
    /// Interaction logic for GamesSelectionWindow.xaml
    /// </summary>
    public partial class GamesSelectionWindow : Window
    {
        private User currentPlayer = null;
        private bool forceClose = true;
        public GamesSelectionWindow(User player)
        {
            InitializeComponent();
            currentPlayer = player;
            CreateTable();
        }

        void CreateTable() 
        {
            SavedGamesList.Items.Clear();
            for (int i = 0; i < currentPlayer.SavedGames.Count; i++)
            {
                ListBoxItem game = new ListBoxItem();
                game.Content = "Game index: " + (i + 1).ToString() + "                Current level: " + currentPlayer.SavedGames[i].CurrentLevel.ToString() + "                Board size: " + currentPlayer.SavedGames[i].CurrentBoard.BoardWidth.ToString() + "x" + currentPlayer.SavedGames[i].CurrentBoard.BoardHeight.ToString();
                game.HorizontalAlignment = HorizontalAlignment.Center;
                game.HorizontalContentAlignment = HorizontalAlignment.Center;
                game.FontSize = 30;
                game.MouseDoubleClick += Button_DoubleClick;
                SavedGamesList.Items.Add(game);
            }  
        }

        private void Button_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            forceClose = false;
            int selectedGameIndex = SavedGamesList.SelectedIndex;
            Game selectedGame = currentPlayer.SavedGames[selectedGameIndex];
            GameWindow gameWindow = new GameWindow(currentPlayer, selectedGame.CurrentBoard.BoardWidth, selectedGame.CurrentBoard.BoardHeight, selectedGame, selectedGameIndex);
            gameWindow.Show();
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(forceClose)
            {
                MainWindow mainWindow = new MainWindow(currentPlayer);
                mainWindow.Show();
            }
        }
    }
}
