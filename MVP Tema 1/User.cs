﻿using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace MVP_Tema_1
{
    [Serializable()]
    public class User : ISerializable
    {
        private string username;
        private string photo;
        private int playedGames;
        private int winnedGames;
        private List<Game> savedGames;

        public User() { }
        public User(string username, string photo)
        {
            this.username = username;
            this.photo = photo;
            playedGames = 0;
            winnedGames = 0;
            savedGames = new List<Game>();
        }
        
        public string UserName
        { 
            get { return username; }
            set { username = value; }
        }

        public string Photo
        {
            get { return photo; }
            set { photo = value; }
        }

        public int PlayedGames
        {
            get { return playedGames; }
            set { playedGames = value; }
        }
        
        public int WinnedGames
        {
            get { return winnedGames; }
            set { winnedGames = value; }
        }

        public List<Game> SavedGames
        {   get { return savedGames; }
            set { savedGames = value; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("username", username);
            info.AddValue("photo", photo);
            info.AddValue("playedGames", playedGames);
            info.AddValue("winnedGames", winnedGames);
            info.AddValue("savedGames", savedGames);
        }

        public User(SerializationInfo info, StreamingContext context)
        {
            UserName = (string)info.GetValue("username", typeof(string));
            Photo = (string)info.GetValue("photo", typeof(string));
            PlayedGames = (int)info.GetValue("playedGames", typeof(int));
            WinnedGames = (int)info.GetValue("winnedGames", typeof(int));
            SavedGames = (List<Game>)info.GetValue("savedGames", typeof(List<Game>));
        }
    }
}
