using System;

namespace Sea_Battle
{
    class Player
    {
        public int shipCount = 10;
        public char[,] logicField =
        {
             {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
             {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
             {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
             {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
             {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
             {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
             {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
             {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
             {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
             {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
        };
        public bool isBot = false;
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
    }
}
