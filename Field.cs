using System;

namespace Sea_Battle
{
    public class Field
    {
        public char[,] array = new char[10, 10];

        public void Create(Player player)
        {
            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    array[x, y] = ' ';
                }
            }

            Generate(player);
        }

        private void Generate(Player player)
        {
            Random rnd = new Random();

            int boatCounter = 0;

            for (int i = 0; boatCounter < player.shipCount; i++)
            {
                int randomX = rnd.Next(0, 10);
                int randomY = rnd.Next(0, 10);

                if (array[randomX, randomY] == ' ')
                {
                    array[randomX, randomY] = '■';
                    boatCounter++;
                }
            }
        }

        public char TryWriteShip(int x, int y, bool isShow)
        {
            if (array[x, y] == '■' && !isShow)
            {
                return ' ';
            }
            else
            {
                return array[x, y];
            }
        }
    }
}
