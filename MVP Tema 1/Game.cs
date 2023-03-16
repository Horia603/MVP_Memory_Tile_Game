using System;
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

        public Game()
        {
            currentLevel = 1;
        }

        public Game(int currentLevel)
        {
            this.currentLevel = currentLevel;
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
