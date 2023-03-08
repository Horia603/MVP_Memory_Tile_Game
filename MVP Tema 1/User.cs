
namespace MVP_Tema_1
{
    internal class User
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
    }
}
