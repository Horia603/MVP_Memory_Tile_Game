using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MVP_Tema_1
{
    /// <summary>
    /// Interaction logic for BoardSizeSelectionWindow.xaml
    /// </summary>
    public partial class BoardSizeSelectionWindow : Window
    {
        private List<ComboBoxItem> widthItemList = new List<ComboBoxItem>();
        private List<ComboBoxItem> heightItemList = new List<ComboBoxItem>();
        private ComboBoxItem selectWidth = null;
        private ComboBoxItem selectHeight = null;
        private User currentUser = null;
        private bool forceClose = true;
        private static int maxSize = 7;
        public BoardSizeSelectionWindow(User user)
        {
            InitializeComponent();

            selectWidth = SetSelector(WidthSelector, widthItemList);
            selectHeight = SetSelector(HeightSelector, heightItemList);
            currentUser = user;
        }

        private ComboBoxItem SetSelector(ComboBox Selector, List<ComboBoxItem> itemList)
        {
            Selector.Items.Clear();
            ComboBoxItem selectItem = new ComboBoxItem();
            selectItem = new ComboBoxItem();
            selectItem.FontSize = 20;
            selectItem.Content = "Select size";
            selectItem.Visibility = Visibility.Collapsed;
            Selector.Items.Add(selectItem);
            Selector.SelectedItem = selectItem;
            for (int i = 1; i < maxSize; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.FontSize = 20;
                item.Content = (i + 1).ToString();
                itemList.Add(item);
                Selector.Items.Add(item);
            }
            return selectItem;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (WidthSelector.SelectedItem == selectWidth || HeightSelector.SelectedItem == selectHeight)
            {
                MessageBox.Show("Please select a valid board size", "Invalid board size");
                return;
            }
            int boardWidth = widthItemList.FindIndex(item => item == WidthSelector.SelectedItem) + 2;
            int boardHeight = heightItemList.FindIndex(item => item == HeightSelector.SelectedItem) + 2;
            GameWindow gameWindow = new GameWindow(currentUser, boardWidth, boardHeight);
            gameWindow.Show();
            forceClose = false;
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(forceClose)
            {
                MainWindow mainWindow = new MainWindow(currentUser);
                mainWindow.Show();
            }
        }
    }
}
