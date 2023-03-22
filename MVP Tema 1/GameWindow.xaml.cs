using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xml.Linq;

namespace MVP_Tema_1
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private User currentPlayer;
        private Game currentGame;
        private bool stopTimeBar = false;
        private int boardSize;
        private Button flipedTile = null;
        private Button jokerTile = null;
        private Tuple<int, int> flipedTilePosition = null;
        private string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
        private bool forceClose = true;
        public GameWindow(User player, int size, Game game = null)
        {
            InitializeComponent();
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
            currentPlayer = player;
            boardSize = size;
            if (game == null)
                currentGame = new Game(1, boardSize);
            else
                currentGame = game;
            if (currentGame.CurrentLevel == 1)
                currentPlayer.PlayedGames++;
            PlayerName.Text = currentPlayer.UserName;
            CurrentLevel.Text = "Level " + currentGame.CurrentLevel.ToString() + "/3";
            string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "Resource\\ProfilePhotos\\" + currentPlayer.Photo));
            PlayerImage.Source = new BitmapImage(new Uri(filePath, UriKind.Absolute));
            DataContext = this;
            CreateTable(boardSize);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to exit the game?", "Close Window", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                MainWindow window = new MainWindow(currentPlayer);
                window.Show();
                Close();
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;

            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int totalSeconds = 60;
            int progressInterval = 100 / totalSeconds;

            for (int i = 0; i <= totalSeconds; i++)
            {
                if (stopTimeBar)
                    break;
                (sender as BackgroundWorker).ReportProgress(i * progressInterval);
                Thread.Sleep(1000);
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            TimeBar.Value = e.ProgressPercentage;
            if (TimeBar.Value == TimeBar.Maximum)
            {
                stopTimeBar = true;
                MessageBox.Show("Time is up!", "Game Over");
                MainWindow mainWindow = new MainWindow(currentPlayer);
                mainWindow.Show();
                Close();
            }
        }

        private void CreateTable(int n)
        {
            for (int row = 0; row < n; row++)
            {
                for (int col = 0; col < n; col++)
                {
                    Button button = new Button();
                    if (currentGame.CurrentBoard.BoardMatrix[row][col].Image == "joker.png")
                    {
                        jokerTile = button;
                    }
                    button.Name = "Button_" + row + "_" + col;
                    button.Content = "?";
                    button.FontSize = 500 / boardSize;
                    button.Click += Button_Click;
                    button.Background = new SolidColorBrush(Color.FromRgb(0, 0, 255));
                    button.HorizontalAlignment = HorizontalAlignment.Stretch;
                    button.VerticalAlignment = VerticalAlignment.Stretch;
                    button.Margin = new Thickness(5);
                    button.Template = CreateButtonTemplate();

                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);

                    Table.Children.Add(button);
                }
            }
        }
        private void InableDisableButtons(bool isInabled)
        {
            foreach (var child in Table.Children)
            {
                if (child is Button button)
                {
                    button.IsEnabled = isInabled;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedTile = sender as Button;

            int row = Grid.GetRow(clickedTile);
            int column = Grid.GetColumn(clickedTile);
            string jokerPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "Resource\\TilesPhotos\\joker.png"));
            string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "Resource\\TilesPhotos\\OtherTiles"));

            if(flipedTile == null)
            {
                flipedTile = clickedTile;
                flipedTilePosition = new Tuple<int, int>(row, column);
                Image image = new Image();
                if(currentGame.CurrentBoard.BoardMatrix[row][column].Image == "joker.png")
                {
                    image.Source = new BitmapImage(new Uri(jokerPath, UriKind.Absolute));
                }
                else
                {
                    image.Source = new BitmapImage(new Uri(filePath + "\\" + currentGame.CurrentBoard.BoardMatrix[row][column].Image, UriKind.Absolute));
                }
                flipedTile.Content = image;
                flipedTile.IsEnabled = false;
                flipedTile.Background = new SolidColorBrush(Color.FromRgb(0, 0, 100));
            }
            else
            {
                Image image = new Image();
                if (currentGame.CurrentBoard.BoardMatrix[row][column].Image == "joker.png")
                {
                    image.Source = new BitmapImage(new Uri(jokerPath, UriKind.Absolute));
                }
                else
                {
                    image.Source = new BitmapImage(new Uri(filePath + "\\" + currentGame.CurrentBoard.BoardMatrix[row][column].Image, UriKind.Absolute));
                }
                clickedTile.Content = image;
                clickedTile.IsEnabled = false;
                clickedTile.Background = new SolidColorBrush(Color.FromRgb(0, 0, 100));
                if(!currentGame.CurrentBoard.CompareTiles(flipedTilePosition, new Tuple<int, int>(row, column)))
                {
                    InableDisableButtons(false);
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromMilliseconds(100);
                    timer.Tick += (s, args) =>
                    {
                        clickedTile.Content = "?";
                        clickedTile.Background = new SolidColorBrush(Color.FromRgb(0, 0, 255));
                        flipedTile.Content = "?";
                        flipedTile.Background = new SolidColorBrush(Color.FromRgb(0, 0, 255));
                        flipedTile = null;
                        flipedTilePosition = null;
                        InableDisableButtons(true);
                        timer.Stop();
                    };
                    timer.Start();
                }
                else
                {
                    currentGame.CurrentBoard.FlipTile(flipedTilePosition);
                    currentGame.CurrentBoard.FlipTile(new Tuple<int, int>(row, column));
                    flipedTile = null;
                    flipedTilePosition = null;
                    if(currentGame.CheckGameEnding())
                    {
                        if(jokerTile != null)
                        {
                            Image jokerImage = new Image();
                            jokerImage.Source = new BitmapImage(new Uri(jokerPath, UriKind.Absolute));
                            jokerTile.Content = jokerImage;
                            jokerTile.IsEnabled = false;
                            jokerTile.Background = new SolidColorBrush(Color.FromRgb(0, 0, 100));
                        }

                        stopTimeBar = true;
                        MessageBox.Show("Level " + currentGame.CurrentLevel.ToString() + " completed", "Level Completed");
                        if (currentGame.CurrentLevel < 3)
                        {
                            GameWindow gameWindow = new GameWindow(currentPlayer, boardSize, new Game(++currentGame.CurrentLevel, boardSize));
                            gameWindow.Show();
                        }
                        else
                        {
                            currentPlayer.WinnedGames++;
                            forceClose = false;
                            SaveGame();
                            MainWindow mainWindow = new MainWindow(currentPlayer);
                            mainWindow.Show();
                        }
                            
                        Close();
                    }
                }
            }
        }

        private List<User> GetUsers()
        {
            string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "Resource\\BinaryFiles\\Users.dat"));

            List<User> users = new List<User>();
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                users = (List<User>)formatter.Deserialize(fileStream);
            }
            return users;
        }

        void SaveGame()
        {
            string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "Resource\\BinaryFiles\\Users.dat"));

            //currentPlayer.SavedGames.Add(currentGame);
            List<User> users = GetUsers();
            for (int i = 0; i < users.Count; i++) 
            {
                if (users[i].UserName == currentPlayer.UserName)
                {
                    users[i] = currentPlayer;
                    break;
                }
            }

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, users);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            stopTimeBar = true;
            if (forceClose)
            {
                SaveGame();
            }
        }

        ControlTemplate CreateButtonTemplate()
        {
            SolidColorBrush disabledBackgroundBrush = new SolidColorBrush(Color.FromRgb(0, 0, 100));

            ControlTemplate customButtonTemplate = new ControlTemplate(typeof(Button));
            FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
            border.Name = "border";
            border.SetBinding(Border.BackgroundProperty, new Binding("Background") { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) });
            border.SetBinding(Border.BorderBrushProperty, new Binding("BorderBrush") { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) });
            border.SetBinding(Border.BorderThicknessProperty, new Binding("BorderThickness") { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) });
            border.SetValue(Border.SnapsToDevicePixelsProperty, true);
            FrameworkElementFactory contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetValue(ContentPresenter.MarginProperty, new Binding("Padding") { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) });
            contentPresenter.SetBinding(ContentPresenter.HorizontalAlignmentProperty, new Binding("HorizontalContentAlignment") { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) });
            contentPresenter.SetBinding(ContentPresenter.VerticalAlignmentProperty, new Binding("VerticalContentAlignment") { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) });
            contentPresenter.SetValue(ContentPresenter.RecognizesAccessKeyProperty, true);
            border.AppendChild(contentPresenter);
            customButtonTemplate.VisualTree = border;

            Trigger disabledTrigger = new Trigger() { Property = UIElement.IsEnabledProperty, Value = false };
            disabledTrigger.Setters.Add(new Setter(Border.BackgroundProperty, disabledBackgroundBrush));
            customButtonTemplate.Triggers.Add(disabledTrigger);

            return customButtonTemplate;
        }
    }
}
