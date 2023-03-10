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
        private string[] photos = null;
        string defaultPhoto = null;
        private int currentImageIndex = 0;
        List<User> users = new List<User>();
        User newUser = new User();
        public CreateUserWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            GetImages();
            GetUsers();
            ProfilePicture.Source = new BitmapImage(new Uri(photos[0], UriKind.Absolute));
        }

        private void GetImages()
        {
            string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "Resource\\ProfilePhotos"));
            photos = Directory.GetFiles(filePath, "*.png");
            List<string> photoList = photos.ToList<string>();
            defaultPhoto = photoList.Find(item => item.Contains("user.png"));
            photoList.Remove(defaultPhoto);
            photos = photoList.ToArray();
            photoList.Clear();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentImageIndex > 0)
            {
                currentImageIndex--;
            }
            else
            {
                currentImageIndex = photos.Length - 1;
            }
            ProfilePicture.Source = new BitmapImage(new Uri(photos[currentImageIndex], UriKind.Absolute));

        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentImageIndex < photos.Length - 1)
            {
                currentImageIndex++;
            }
            else
            {
                currentImageIndex = 0;
            }
            ProfilePicture.Source = new BitmapImage(new Uri(photos[currentImageIndex], UriKind.Absolute));
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
            if(UserNameBox.Text == "Enter username" || UserNameBox.Text == "New User" || UserNameBox.Text == "Select User")
            {
                MessageBox.Show("Invalid username", "Invalid username");
                return;
            }

            foreach(var user in users)
            {
                if(user.UserName == UserNameBox.Text)
                {
                    MessageBox.Show("Username already tacken", "Username tacken");
                    return;
                }
            }

            newUser.UserName = UserNameBox.Text;
            int lastIndex = ProfilePicture.Source.ToString().LastIndexOf('/');
            newUser.Photo = ProfilePicture.Source.ToString().Substring(lastIndex + 1);
            users.Add(newUser);

            string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "Resource\\BinaryFiles\\Users.dat"));

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, users);
            }
            this.DialogResult = true;
            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (mainWindow != null)
            {
                mainWindow.Refresh();
            }

            Close();
        }

        private void GetUsers()
        {
            string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "Resource\\BinaryFiles\\Users.dat"));

            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    this.users = (List<User>)formatter.Deserialize(fileStream);
                }
            }
            catch
            {
                this.users = new List<User>();
            }
        }
    }
}
