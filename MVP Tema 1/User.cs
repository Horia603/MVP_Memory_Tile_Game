using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MVP_Tema_1
{
    [Serializable()]
    internal class User : ISerializable
    {
        private string username;
        private string photo;

        public User() { }
        public User(string username, string photo)
        {
            this.username = username;
            this.photo = photo;
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("username", username);
            info.AddValue("photo", photo);
        }

        public User(SerializationInfo info, StreamingContext context)
        {
            UserName = (string)info.GetValue("username", typeof(string));
            Photo = (string)info.GetValue("photo", typeof(string));
        }
    }
}
