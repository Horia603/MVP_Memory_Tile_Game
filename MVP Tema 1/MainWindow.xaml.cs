using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MVP_Tema_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] filePaths = null;
        int currentImageIndex = 0;
        ComboBoxItem newUser = new ComboBoxItem();
        string currentUser = null;
        public MainWindow()
        {
            InitializeComponent();
            GetImages();
            ProfilePicture.Source = new BitmapImage(new Uri(filePaths[currentImageIndex], UriKind.Absolute));
 
            newUser.FontSize = 20;
            newUser.Content = "New User";
            UserSelector.Items.Add(newUser);

            ComboBoxItem user1 = new ComboBoxItem();
            user1.FontSize = 20;
            user1.Content = "User1";
            UserSelector.Items.Add(user1);

            ComboBoxItem user2 = new ComboBoxItem();
            user2.FontSize = 20;
            user2.Content = "User2";
            UserSelector.Items.Add(user2);

            ComboBoxItem user3 = new ComboBoxItem();
            user3.FontSize = 20;
            user3.Content = "User3";
            UserSelector.Items.Add(user3);

            ComboBoxItem user4 = new ComboBoxItem();
            user4.FontSize = 20;
            user4.Content = "User4";
            UserSelector.Items.Add(user4);

            ComboBoxItem user5 = new ComboBoxItem();
            user5.FontSize = 20;
            user5.Content = "User5";
            UserSelector.Items.Add(user5);

            ComboBoxItem user6 = new ComboBoxItem();
            user6.FontSize = 20;
            user6.Content = "User6";
            UserSelector.Items.Add(user6);

            ComboBoxItem user7 = new ComboBoxItem();
            user7.FontSize = 20;
            user7.Content = "User7";
            UserSelector.Items.Add(user7);

            ComboBoxItem user8 = new ComboBoxItem();
            user8.FontSize = 20;
            user8.Content = "User8";
            UserSelector.Items.Add(user8);

            ComboBoxItem user9 = new ComboBoxItem();
            user9.FontSize = 20;
            user9.Content = "User9";
            UserSelector.Items.Add(user9);

            ComboBoxItem user10 = new ComboBoxItem();
            user10.FontSize = 20;
            user10.Content = "User10";
            UserSelector.Items.Add(user10);
        }

        private void GetImages()
        {
            string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "Resources"));
            filePaths = Directory.GetFiles(filePath, "*.png");
        }

        private void UserName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(UserSelector.SelectedItem.Equals(newUser))
            {
                CreateUserWindow createUserWindow = new CreateUserWindow();
                createUserWindow.ShowDialog();
            }
            currentUser = UserSelector.SelectedItem.ToString();
        }
    }
}
