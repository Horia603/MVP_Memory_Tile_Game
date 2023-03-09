using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;

namespace MVP_Tema_1
{
    /// <summary>
    /// Interaction logic for CreateUserWindow.xaml
    /// </summary>
    public partial class CreateUserWindow : Window
    {
        private string[] filePaths = null;
        private int currentImageIndex = 0;
        List<User> users = new List<User>();
        User newUser = new User();
        public CreateUserWindow()
        {
            InitializeComponent();
            GetImages();
            GetUsers();
            ProfilePicture.Source = new BitmapImage(new Uri(filePaths[currentImageIndex], UriKind.Absolute));
            Closing += Window_Closing;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (mainWindow != null)
            {
                mainWindow.Refresh();
            }
        }

        private void GetImages()
        {
            string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "Resource\\ProfilePhotos"));
            filePaths = Directory.GetFiles(filePath, "*.png");
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentImageIndex > 0)
            {
                currentImageIndex--;
                ProfilePicture.Source = new BitmapImage(new Uri(filePaths[currentImageIndex], UriKind.Absolute));
            }

        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentImageIndex < filePaths.Length - 1)
            {
                currentImageIndex++;
                ProfilePicture.Source = new BitmapImage(new Uri(filePaths[currentImageIndex], UriKind.Absolute));
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            UserNameBox.Text = "";
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (UserNameBox.Text == "")
            {
                UserNameBox.Text = "Enter username";
            }
        }

        private void CreateAcount_Click(object sender, RoutedEventArgs e)
        {
            newUser.UserName = UserNameBox.Text;
            newUser.Photo = ProfilePicture.Source.ToString();

            users.Add(newUser);

            using (FileStream fileStream = new FileStream("Resources\\BinaryFiles\\Users.dat", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, users);
            }

            Close();
        }

        private void GetUsers()
        {
            using (FileStream fileStream = new FileStream("Resources\\BinaryFiles\\Users.dat", FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                this.users = (List<User>)formatter.Deserialize(fileStream);
            }
        }
    }
}
