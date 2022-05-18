using System;

namespace Sea_Battle
{
    public class Player
    {
        public Field field = new Field();
        public int shipCount;
        public bool isBot;
        public int score;
        public ConsoleColor color;

        public void SetDefaultValues(int numberOfship, ConsoleColor color, int score)
        {
            shipCount = numberOfship;
            this.color = color;
            this.score = score;
        }

        public int Attack(int x, int y)
        {
            if (field.array[x, y] == '■')
            {
                field.array[x, y] = 'X';
                shipCount -= 1;
            }
            else if (field.array[x, y] == ' ')
            {
                field.array[x, y] = '·';
            }

            return shipCount;
        }
    }
}
