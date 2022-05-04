using System;
using System.Threading;

public class Game
{
    private char[,] field = 
    {
        {' ', ' ', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ' '},
        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
        {' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
        {'A', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#'},
        {'B', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#'},
        {'C', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#'},
        {'D', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#'},
        {'E', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#'},
        {'F', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#'},
        {'G', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#'},
        {'H', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#'},
        {'I', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#'},
        {'J', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#'},
        {' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
    };

    private int firstCounter = 10;
    private int secondCounter = 10;

    private int currentPlayer = 0;

    private ConsoleKey[] xKeysArray = {ConsoleKey.A, ConsoleKey.B, ConsoleKey.C, ConsoleKey.D, ConsoleKey.E, ConsoleKey.F, ConsoleKey.G, ConsoleKey.H, ConsoleKey.I, ConsoleKey.J};

    private ConsoleKey[] yKeysArray = {ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4, ConsoleKey.D5, ConsoleKey.D6, ConsoleKey.D7, ConsoleKey.D8, ConsoleKey.D9, ConsoleKey.D0};

    public void GameLoop()
    {
        var fieldParams = CreateFields();
        while (!isEndGame())
        {
            UpdateFields(fieldParams);
            CurrentPlayerTurn(fieldParams);
            Console.Clear();
        }
    }

    private (int, int, char[,], char[,]) CreateFields()
    {
        int fieldWidth = field.GetLength(0);
        int fieldHeight = field.GetLength(1);

        char[,] firstField = (char[,])field.Clone();
        char[,] secondField = (char[,])field.Clone();

        firstField = GenerateShips(firstField);
        Thread.Sleep(10);
        secondField = GenerateShips(secondField);

        DrawFields(fieldHeight, fieldWidth, firstField, secondField, false);

        return (fieldHeight, fieldWidth, firstField, secondField);
    }

    private char[,] GenerateShips(char[,] field)
    {
        Random rnd = new Random();

        int boatCounter = 0;

        for (int i = 0; boatCounter < firstCounter; i++)
        {
            int randomX = rnd.Next(3, 13);
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
                if (isAnim) Thread.Sleep(10);
            }
            Console.Write("          ");

            for (int i = 0; i < fieldWidth; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                if (secondField[i, j] is '■') Console.Write(" ");
                else Console.Write(secondField[i, j]);

                if (isAnim) Thread.Sleep(10);
            }
            Console.WriteLine();
        }

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.Write(" Player 1 field");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("          Player 2 field");
    }

    private void CurrentPlayerTurn((int, int, char[,], char[,]) fieldParams)
    {
        if(currentPlayer % 2 == 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("              Player 1 Turn");
            TryPlayer1Attack(fieldParams.Item4);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("              Player 2 Turn");
            TryPlayer2Attack(fieldParams.Item3);
        }

        currentPlayer++;
    }

    private void UpdateFields((int, int, char[,], char[,]) fieldParams)
    {
        Console.Clear();
        DrawFields(fieldParams.Item1, fieldParams.Item2, fieldParams.Item3, fieldParams.Item4, false);
    }

    private void TryPlayer1Attack(char[,] secondField)
    {
        int x = CalculateAxis("\n" + "Write the coordinate of X (A, B, C, D, E ... J)", xKeysArray);
        int y = CalculateAxis("\n" + "Write the coordinate of Y (1, 2, 3, 4, 5 ... 10)", yKeysArray) - 1;

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

    private void TryPlayer2Attack(char[,] firstField)
    {
        Random rnd = new Random();

        Thread.Sleep(rnd.Next(1000, 5000));

        int x = rnd.Next(3, 13);
        int y = rnd.Next(2, 12);

        if (firstField[x, y] == '■')
        {
            firstField[x, y] = 'X';
            firstCounter -= 1;
        }
        else if (firstField[x, y] == ' ')
        {
            firstField[x, y] = '·';
        }
    }

    private bool isEndGame()
    {
        if (firstCounter is 0) 
        {
            EndGame("Player 2", ConsoleColor.Red);
            return true;
        }
        else if (secondCounter is 0)
        {
            EndGame("Player 1", ConsoleColor.DarkBlue);
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

    private int CalculateAxis(string massage, ConsoleKey[] keysArray)
    {
        Console.WriteLine(massage);    

        int a;

        do
        {
            a = Input(keysArray);

            if (a == 0)
            {
                Console.WriteLine("Wrong, try again!");
            }

        } while (a == 0);

        return a + 2;
    }

    private int Input(ConsoleKey[] keysArray)
    {
        var input = Console.ReadKey();

        for (int i = 0; i < keysArray.Length; i++)
        {
            if (input.Key == keysArray[i])
            {
                return i + 1;
            }
        }
        return 0;      
    }
}