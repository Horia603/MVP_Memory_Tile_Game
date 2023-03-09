using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
        List<User> users = new List<User>();
        string currentUser = null;  
        public MainWindow()
        {
            InitializeComponent();
            GetImages();
            GetUsers();
            ProfilePicture.Source = new BitmapImage(new Uri(filePaths[0], UriKind.Absolute));
            AddUsersToSelector();
        }

        private void GetImages()
        {
            string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "Resource\\ProfilePhotos"));
            filePaths = Directory.GetFiles(filePath, "*.png");
        }

        private void UserName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(UserSelector.SelectedItem != null)
            {
                if (UserSelector.SelectedItem.Equals(newUser))
                {
                    CreateUserWindow createUserWindow = new CreateUserWindow();
                    createUserWindow.ShowDialog();
                }
                currentUser = UserSelector.SelectedItem.ToString();
            }
        }

        public void GetUsers()
        {
            using (FileStream fileStream = new FileStream("Resources\\BinaryFiles\\Users.dat", FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                this.users = (List<User>)formatter.Deserialize(fileStream);
            }
        }

        public void AddUsersToSelector()
        {
            UserSelector.Items.Clear();

            newUser = new ComboBoxItem();
            newUser.FontSize = 20;
            newUser.Content = "New User";
            UserSelector.Items.Add(newUser);

            foreach (User user in users)
            {
                ComboBoxItem userItem = new ComboBoxItem();
                userItem = new ComboBoxItem();
                userItem.FontSize = 20;
                userItem.Content = user.UserName;
                UserSelector.Items.Add(userItem);
            }
        }

        public void Refresh()
        {
            GetUsers();
            AddUsersToSelector();
            UserSelector.SelectedItem = UserSelector.Items[UserSelector.Items.Count - 1];
        }
    }
}
