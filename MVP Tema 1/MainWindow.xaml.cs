﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private string[] photos = null;
        string defaultPhoto = null;
        ComboBoxItem newUser = new ComboBoxItem();
        ComboBoxItem selectUser = new ComboBoxItem();
        List<User> users = new List<User>();
        User currentUser = null;  
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            GetImages();
            GetUsers();
            ProfilePicture.Source = new BitmapImage(new Uri(defaultPhoto, UriKind.Absolute));
            newUser.FontSize = 20;
            newUser.Content = "New User";
            selectUser.FontSize = 20;
            selectUser.Content = "Select User";
            selectUser.Visibility = Visibility.Collapsed;
            AddUsersToSelector();
            UserSelector.SelectedItem = selectUser;
        }

        public MainWindow(User user)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            GetImages();
            GetUsers();
            newUser.FontSize = 20;
            newUser.Content = "New User";
            selectUser.FontSize = 20;
            selectUser.Content = "Select User";
            selectUser.Visibility = Visibility.Collapsed;
            AddUsersToSelector();
            SetCurrentUser(user);
        }

        private void SetCurrentUser(User user)
        {
            currentUser = user;
            string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "Resource\\ProfilePhotos\\" + currentUser.Photo));
            ProfilePicture.Source = new BitmapImage(new Uri(filePath, UriKind.Absolute));
            foreach (ComboBoxItem userItem in UserSelector.Items)
            {
                if (userItem.Content.ToString() == currentUser.UserName)
                {
                    UserSelector.SelectedItem = userItem;
                }
            }
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

        private void UserName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(UserSelector.SelectedItem == null)
            {
                return;
            }

            if (UserSelector.SelectedItem.Equals(newUser))
            {
                CreateUserWindow createUserWindow = new CreateUserWindow();
                createUserWindow.ShowDialog();
                if(createUserWindow.DialogResult == false)
                {
                    UserSelector.SelectedItem = null;
                    UserSelector.Text = "Select User";
                    ProfilePicture.Source = new BitmapImage(new Uri(defaultPhoto, UriKind.Absolute));
                    return;
                }
            }

            foreach(User user in users)
            {
                ComboBoxItem selectedUser = (ComboBoxItem)UserSelector.SelectedItem;
                ComboBoxItem savedUser = new ComboBoxItem();
                savedUser.Content = user.UserName;

                if (savedUser.Content == selectedUser.Content)
                {
                    currentUser = user;
                    string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
                    string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "Resource\\ProfilePhotos\\" + currentUser.Photo));
                    ProfilePicture.Source = new BitmapImage(new Uri(filePath, UriKind.Absolute));
                }
            }
        }

        public void GetUsers()
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

        public void AddUsersToSelector()
        {
            UserSelector.Items.Clear();
            UserSelector.Items.Add(newUser);
            UserSelector.Items.Add(selectUser);

            foreach (User user in users)
            {
                ComboBoxItem userItem = new ComboBoxItem();
                userItem.FontSize = 20;
                userItem.Content = user.UserName;
                UserSelector.Items.Add(userItem);
            }
        }

        public void Refresh(int itemIndex = 0)
        {
            GetUsers();
            AddUsersToSelector();

            if (itemIndex == 0)
            {
                itemIndex = UserSelector.Items.Count - 1;
            }

            if (itemIndex != -1)
            {
                UserSelector.SelectedItem = UserSelector.Items[itemIndex];
            }
            else
            {
                UserSelector.Text = "Select User";
            }
            ProfilePicture.Source = new BitmapImage(new Uri(defaultPhoto, UriKind.Absolute));
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if(UserSelector.SelectedItem == null || currentUser == null || currentUser.Equals(newUser))
            {
                return;
            }

            users.Remove(currentUser);

            string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "Resource\\BinaryFiles\\Users.dat"));

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, users);
            }

            Refresh(-1);
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserSelector.SelectedItem == selectUser)
            {
                MessageBox.Show("Select or create an account", "No user selected");
                return;
            }
            BoardSizeSelectionWindow boardSizeSelectionWindow = new BoardSizeSelectionWindow(currentUser);
            boardSizeSelectionWindow.Show();
            Close();
        }

        private void LoadGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserSelector.SelectedItem == selectUser)
            {
                MessageBox.Show("Select or create an account", "No user selected");
                return;
            }
            GamesSelectionWindow gamesSelectionWindow = new GamesSelectionWindow(currentUser);
            gamesSelectionWindow.Show();
            Close();
        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserSelector.SelectedItem == selectUser)
            {
                MessageBox.Show("Select or create an account", "No user selected");
                return;
            }
            StatisticsWindow statisticsWindow = new StatisticsWindow(currentUser);
            statisticsWindow.Show();
            Close();
        }
    }
}
