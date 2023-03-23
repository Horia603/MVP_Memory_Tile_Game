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
        public GamesSelectionWindow(User player)
        {
            InitializeComponent();
            currentPlayer = player;
            CreateTable();
            Viewer.ScrollToVerticalOffset(Viewer.VerticalOffset + (Viewer.ViewportHeight * 3));
        }

        void CreateTable() 
        {
            Table.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < currentPlayer.SavedGames.Count; i++)
            {
                Table.RowDefinitions.Add(new RowDefinition());

                Button game = new Button();
                game.Content = "Game index: " + (i + 1).ToString() + "                Current level: " + currentPlayer.SavedGames[i].CurrentLevel.ToString() + "                Board size: " + currentPlayer.SavedGames[i].CurrentBoard.BoardWidth.ToString() + "x" + currentPlayer.SavedGames[i].CurrentBoard.BoardHeight.ToString();
                game.HorizontalAlignment = HorizontalAlignment.Center;
                game.HorizontalContentAlignment = HorizontalAlignment.Center;
                game.Width = 900;
                game.FontSize = 30;
                game.Click += Button_Click;

                Grid.SetRow(game, i);
                Grid.SetColumn(game, 0);
                Table.Children.Add(game);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
