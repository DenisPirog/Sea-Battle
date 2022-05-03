using System;
using System.Threading;

public class Game
{
    private string[] field =
        {
            "    ABCDEFGHIJ  ",
            "   ############",
            " 1 #          #",
            " 2 #          #",
            " 3 #          #",
            " 4 #          #",
            " 5 #          #",
            " 6 #          #",
            " 7 #          #",
            " 8 #          #",
            " 9 #          #",
            "10 #          #",
            "   ############",
        };
    private int firstCounter = 10;
    private int secondCounter = 10;

    public void GameLoop()
    {
        var fieldParams = CreateFields();
        while (!isEndGame())
        {
            PlayerTurn(fieldParams);
            if (isEndGame()) break;
            EnemyTurn(fieldParams);
            Console.Clear();
        }
    }

    private bool isEndGame()
    {
        if (firstCounter is 0)
        {
            EndGame("Enemy", ConsoleColor.Red);
            return true;
        }
        else if (secondCounter is 0)
        {
            EndGame("Player", ConsoleColor.DarkBlue);
            return true;
        }
        return false;
    }

    private void EndGame(string winner, ConsoleColor color)
    {
        Console.Clear();
        Console.ForegroundColor = color;
        Console.WriteLine(winner + " wins!");
    }

    private (int, int, char[,], char[,]) CreateFields()
    {
        int fieldWidth = field[0].Length;
        int fieldHeight = field.Length;

        char[,] firstField = new char[fieldWidth, fieldHeight];
        for (int i = 0; i < fieldHeight; i++)
        {
            for (int j = 0; j < fieldWidth - 1; j++)
            {
                string line = field[i];
                firstField[j, i] = line[j];
            }
        }
        char[,] secondField = new char[fieldWidth, fieldHeight];
        for (int i = 0; i < fieldHeight; i++)
        {
            for (int j = 0; j < fieldWidth - 1; j++)
            {
                string line = field[i];
                secondField[j, i] = line[j];
            }
        }

        firstField = GenerateShips(firstField);
        Thread.Sleep(10);
        secondField = GenerateShips(secondField);

        DrawFields(fieldHeight, fieldWidth, firstField, secondField, true);

        var result = (fieldHeight, fieldWidth, firstField, secondField);
        return result;
    }

    private char[,] GenerateShips(char[,] field)
    {
        Random rnd = new Random();

        int boatCounter = 0;

        for (int i = 0; boatCounter < firstCounter; i++)
        {
            int randomX = rnd.Next(4, 15);
            int randomY = rnd.Next(2, 12);

            if (field[randomX, randomY] is ' ')
            {
                field[randomX, randomY] = '■';
                boatCounter++;
            }
        }

        return field;
    }

    private void DrawFields(int fieldHeight, int fieldWidth, char[,] firstField, char[,] secondField, bool isAnim)
    {
        for (int j = 0; j < fieldHeight; j++)
        {
            for (int i = 0; i < fieldWidth; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write(firstField[i, j]);
                if (isAnim)
                {
                    Thread.Sleep(10);
                }
            }
            Console.Write("          ");

            for (int i = 0; i < fieldWidth; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                if (secondField[i, j] is '■')
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.Write(secondField[i, j]);
                }

                if (isAnim)
                {
                    Thread.Sleep(10);
                }
            }
            Console.WriteLine();
        }

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.Write("   Player field");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("              Enemy field");
        Console.ForegroundColor = ConsoleColor.Green;
    }

    private void TryPlayerAttack(char[,] secondField)
    {
        var coordinates = CalculateCoordinates();
        int x = coordinates.Item1;
        int y = coordinates.Item2;

        if (secondField[x, y] is '■')
        {
            secondField[x, y] = 'X';
            secondCounter -= 1;
        }
        else if (secondField[x, y] is ' ')
        {
            secondField[x, y] = '·';
        }
    }

    private (int, int) CalculateCoordinates()
    {
        char[] letters =
        {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j'
            };
        char[] capitalLetters =
        {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'
            };

        int x = 0;

        Console.WriteLine("\n" + "Write the coordinate of X (A, B, C, D, E ... J)");

        do
        {
            var inputX = Console.ReadLine();
            for (int i = 0; i < letters.Length; i++)
            {
                //if (inputX.ToString() is letters[i].ToString() or capitalLetters[i].ToString())
                // {
                //    x = i + 1;
                // }
            }

            if (x == 0)
            {
                Console.WriteLine("Wrong, try again!");
            }

        } while (x == 0);


        int y = 0;

        Console.WriteLine("\n" + "Write the coordinate of Y (1, 2, 3, 4, 5 ... 10)");

        do
        {
            var inputY = Console.ReadLine();

            if (Int32.TryParse(inputY, out int Y))
            {
                if (Int32.Parse(inputY) > 0 && Int32.Parse(inputY) <= 10)
                {
                    y = Int32.Parse(inputY);
                }
            }

            if (y == 0)
            {
                Console.WriteLine("Wrong, try again!");
            }
        } while (y == 0);

        var coordinates = (x + 3, y + 1);
        return coordinates;
    }

    private void TryEnemyAttack(char[,] firstField)
    {
        Random rnd = new Random();
        bool isRepiating = false;
        Thread.Sleep(rnd.Next(1000, 5000));
        do
        {
            int x = rnd.Next(4, 14);
            int y = rnd.Next(2, 12);

            if (firstField[x, y] == '■')
            {
                firstField[x, y] = 'X';
                firstCounter -= 1;
                isRepiating = false;
            }
            else if (firstField[x, y] == ' ')
            {
                firstField[x, y] = '·';
                isRepiating = false;
            }
            else if (firstField[x, y] == '·' || firstField[x, y] == 'X')
            {
                isRepiating = true;
            }
        } while (isRepiating);
    }

    private void PlayerTurn((int, int, char[,], char[,]) fieldParams)
    {
        Console.Clear();
        DrawFields(fieldParams.Item1, fieldParams.Item2, fieldParams.Item3, fieldParams.Item4, false);
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("                Player Turn");

        TryPlayerAttack(fieldParams.Item4);
    }

    private void EnemyTurn((int, int, char[,], char[,]) fieldParams)
    {
        Console.Clear();
        DrawFields(fieldParams.Item1, fieldParams.Item2, fieldParams.Item3, fieldParams.Item4, false);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("                 Enemy Turn");

        TryEnemyAttack(fieldParams.Item3);
    }
}


