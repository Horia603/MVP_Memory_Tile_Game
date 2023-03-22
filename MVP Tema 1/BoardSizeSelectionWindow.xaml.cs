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
        private List<ComboBoxItem> itemList = new List<ComboBoxItem>();
        ComboBoxItem selectSize = null;
        private User currentUser = null;
        public BoardSizeSelectionWindow(User user)
        {
            InitializeComponent();
            SetSelector();
            currentUser = user;
        }

        private void SetSelector()
        {
            Selector.Items.Clear();
            selectSize = new ComboBoxItem();
            selectSize.FontSize = 20;
            selectSize.Content = "Select size";
            selectSize.Visibility = Visibility.Collapsed;
            Selector.Items.Add(selectSize);
            Selector.SelectedItem = selectSize;
            for (int i = 1; i < 6; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.FontSize = 20;
                item.Content = (i + 1).ToString() + "x" + (i + 1).ToString();
                itemList.Add(item);
                Selector.Items.Add(item);
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (Selector.SelectedItem == selectSize)
            {
                MessageBox.Show("Please select a valid board size", "Invalid board size");
                return;
            }
            int boardSize = itemList.FindIndex(item => item == Selector.SelectedItem) + 2;
            GameWindow gameWindow = new GameWindow(currentUser, boardSize);
            gameWindow.Show();
            Close();
        }
    }
}
