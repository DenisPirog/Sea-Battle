using System;

namespace Sea_Battle
{
    class Player
    {
        public int shipCount;
        public char[,] logicField = new char[10, 10];
        public bool isBot;
        public int score;
        public ConsoleColor color;

        public char TryWriteShip(int x, int y, bool isShow)
        {
            if (logicField[x, y] == '■' && !isShow)
            {
                return ' ';
            }
            else
            {
                return logicField[x, y];
            }
        }

        public bool Attack(int x, int y)
        {
            if (logicField[x, y] == '■')
            {
                logicField[x, y] = 'X';
                shipCount -= 1;
                return true;
            }
            else if (logicField[x, y] == ' ')
            {
                logicField[x, y] = '·';
                return false;
            }

            return false;
        }

        public void SetDefaultValues(int numberOfship)
        {
            shipCount = numberOfship;
            for (int x = 0; x < logicField.GetLength(0); x++)
            {
                for (int y = 0; y < logicField.GetLength(1); y++)
                {
                    logicField[x, y] = ' ';
                }
            }
        }
    }
}
