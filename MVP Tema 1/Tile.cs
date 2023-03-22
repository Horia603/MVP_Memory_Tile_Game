using System;
using System.Runtime.Serialization;

namespace MVP_Tema_1
{
    [Serializable()]
    public class Tile
    {
        private string image;
        private bool visible;

        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public Tile(string image = null)
        {
            this.image = image;
            visible = false;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("image", image);
            info.AddValue("visible", visible);
        }

        public Tile(SerializationInfo info, StreamingContext context)
        {
            Image = (string)info.GetValue("image", typeof(string));
            Visible = (bool)info.GetValue("visible", typeof(bool));
        }

        public void Flip()
        {
            if (visible)
                visible = false;
            else
                visible = true;
        }
    }
}
