using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVP_Tema_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] filePaths = null;
        int currentImageIndex = 0;
        public MainWindow()
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
            if(currentImageIndex > 0)
            {
                currentImageIndex--;
                ProfilePicture.Source = new BitmapImage(new Uri(filePaths[currentImageIndex], UriKind.Absolute));
            }
            
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if(currentImageIndex < filePaths.Length - 1)
            {
                currentImageIndex++;
                ProfilePicture.Source = new BitmapImage(new Uri(filePaths[currentImageIndex], UriKind.Absolute));
            }
            
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void UserNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            UserNameTextBox.Text = string.Empty;
        }

        private void UserNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (UserNameTextBox.Text == string.Empty)
                UserNameTextBox.Text = "Enter your username";
        }
    }
}
