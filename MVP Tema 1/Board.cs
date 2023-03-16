using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MVP_Tema_1
{
    [Serializable()]
    public class Board
    {
        private List<List<Tile>> boardMatrix;
        private int boardDimension;

        public List<List<Tile>> BoardMatrix
        {
            get { return boardMatrix; }
            set { boardMatrix = value; }
        }

        public int BoardDimension
        {
            get { return boardDimension; }
            set { boardDimension = value; }
        }   

        public Board()
        {
            boardDimension = 5;
        }

        public Board(int boardDimension)
        {
            this.boardDimension = boardDimension;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("boardMatrix", boardMatrix);
            info.AddValue("boardDimension", boardDimension);
        }

        public Board(SerializationInfo info, StreamingContext context)
        {
            BoardMatrix = (List<List<Tile>>)info.GetValue("boardMatrix", typeof(List<List<Tile>>));
            BoardDimension = (int)info.GetValue("boardDimension", typeof(int));
        }
    }
}
