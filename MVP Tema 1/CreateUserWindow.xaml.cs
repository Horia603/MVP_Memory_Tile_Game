using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MVP_Tema_1
{
    /// <summary>
    /// Interaction logic for CreateUserWindow.xaml
    /// </summary>
    public partial class CreateUserWindow : Window
    {
        private string[] filePaths = null;
        private int currentImageIndex = 0;
        User user = new User();
        public CreateUserWindow()
        {
            InitializeComponent();
            GetImages();
            ProfilePicture.Source = new BitmapImage(new Uri(filePaths[currentImageIndex], UriKind.Absolute));
        }

        private void GetImages()
        {
            string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "Resources"));
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
            user.UserName = UserNameBox.Text;
            user.Photo = ProfilePicture.Source.ToString();
            Close();
        }
    }
}
