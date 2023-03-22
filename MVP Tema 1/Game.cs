using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MVP_Tema_1
{
    [Serializable()]
    public class Game
    {
        private Board currentBoard;
        private int currentLevel;

        public Board CurrentBoard
        {
            get { return currentBoard; }
            set { currentBoard = value; }
        }

        public int CurrentLevel
        {
            get { return currentLevel; }
            set { currentLevel = value; }
        }

        public Game(int currentLevel = 1, int boardSize = 5)
        {
            this.currentLevel = currentLevel;
            currentBoard = new Board(boardSize);
        }

        public bool CheckGameEnding()
        {
            foreach(List<Tile> tiles in currentBoard.BoardMatrix)
            {
                foreach(Tile tile in tiles)
                {
                    if (tile.Visible == false && tile.Image != "joker.png")
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("currentBoard", currentBoard);
            info.AddValue("currentLevel", currentLevel);
        }

        public Game(SerializationInfo info, StreamingContext context)
        {
            CurrentBoard = (Board)info.GetValue("currentBoard", typeof(Board));
            CurrentLevel = (int)info.GetValue("currentLevel", typeof(int));
        }
    }
}
