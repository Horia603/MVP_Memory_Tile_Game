using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;

namespace MVP_Tema_1
{
    [Serializable()]
    public class Board
    {
        private List<List<Tile>> boardMatrix;
        private int boardWidth;
        private int boardHeight;
        private string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;

        public List<List<Tile>> BoardMatrix
        {
            get { return boardMatrix; }
            set { boardMatrix = value; }
        }

        public int BoardWidth
        {
            get { return boardWidth; }
            set { boardWidth = value; }
        }

        public int BoardHeight
        {
            get { return boardHeight; }
            set { boardHeight = value; }
        }

        public Board(int boardWidth, int boardHeight)
        {
            this.boardWidth = boardWidth;
            this.boardHeight = boardHeight;
            List<Tile> tileList = GetTileList(boardWidth, boardHeight);
            tileList = ShuffleTileList(tileList);
            boardMatrix = new List<List<Tile>>();
            for (int i = 0; i < this.boardHeight; i++)
            {
                boardMatrix.Add(new List<Tile>());
                for (int j = 0; j < this.boardWidth; j++)
                {
                    boardMatrix[i].Add(tileList[0]);
                    tileList.RemoveAt(0);
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("boardMatrix", boardMatrix);
            info.AddValue("boardWidth", boardWidth);
            info.AddValue("boardHeight", boardHeight);
        }

        public Board(SerializationInfo info, StreamingContext context)
        {
            BoardMatrix = (List<List<Tile>>)info.GetValue("boardMatrix", typeof(List<List<Tile>>));
            BoardWidth = (int)info.GetValue("boardWidth", typeof(int));
            BoardHeight = (int)info.GetValue("boardHeight", typeof(int));
        }

        public bool CompareTiles(Tuple<int, int> pozTile1, Tuple<int, int> pozTile2)
        {
            if (boardMatrix[pozTile1.Item1][pozTile1.Item2].Image == boardMatrix[pozTile2.Item1][pozTile2.Item2].Image)
                return true;
            return false;
        }

        public void FlipTile(Tuple<int, int> pozTile)
        {
            boardMatrix[pozTile.Item1][pozTile.Item2].Flip();
        }

        List<Tile> GetTileList(int boardWidth, int boardHeight)
        {
            string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "Resource\\TilesPhotos\\OtherTiles"));
            string[] Sphotos = Directory.GetFiles(filePath, "*.png");
            List<string> photos = new List<string>();
            foreach(string photo in Sphotos)
            {
                int lastIndex = photo.LastIndexOf('\\');
                string photoName = photo.Substring(lastIndex + 1);
                photos.Add(photoName);
            }
            List<Tile> tiles = new List<Tile>();
            if ((boardWidth * boardHeight) % 2 == 1)
            {
                tiles.Add(new Tile("joker.png"));
            }
            for (int i = 0; i < (boardWidth * boardHeight) - 1; i += 2)
            {
                Random rnd = new Random();
                int randomNumber = rnd.Next(photos.Count);
                tiles.Add(new Tile(photos[randomNumber]));
                tiles.Add(new Tile(photos[randomNumber]));
                photos.RemoveAt(randomNumber);
            }
            return tiles;
        }
        //Knuth shuffle algorithm
        List<Tile> ShuffleTileList(List<Tile> tileList)
        {
            Random rng = new Random();
            int n = tileList.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Tile tile = tileList[k];
                tileList[k] = tileList[n];
                tileList[n] = tile;
            }
            return tileList;
        }
    }
}
