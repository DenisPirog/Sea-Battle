using System;
using System.Threading;

namespace Sea_Battle
{
    class Player
    {
        public int shipCount = 10;
        public char[,] field = new char[14,14];
        public bool isBot = false;
        public bool isCurrent = false;
    }

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
        
        private Player player1 =  new Player();
        private Player player2 = new Player();

        private ConsoleKey[] xKeysArray = { ConsoleKey.A, ConsoleKey.B, ConsoleKey.C, ConsoleKey.D, ConsoleKey.E, ConsoleKey.F, ConsoleKey.G, ConsoleKey.H, ConsoleKey.I, ConsoleKey.J };
        private ConsoleKey[] yKeysArray = { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4, ConsoleKey.D5, ConsoleKey.D6, ConsoleKey.D7, ConsoleKey.D8, ConsoleKey.D9, ConsoleKey.D0 };

        public void GameLoop()
        {
            if (isGameModeSelected())
            {
                var fieldParams = CreateFields();

                Player currentPlayer = player1;                
                Player notCurrentPlayer = player2;
                player1.isCurrent = true;

                while (!isEndGame())
                {
                    UpdateFields(fieldParams.Item1, fieldParams.Item2);
                    bool isHit = CurrentPlayerTurn(currentPlayer, notCurrentPlayer);
                    if (!isHit)
                    {
                        var players = ChangeCurrentPlayer();
                        currentPlayer = players.Item1;
                        notCurrentPlayer = players.Item2;
                    }                    
                    Console.Clear();
                }
            }         
        }

        private bool isGameModeSelected()
        {
            Console.WriteLine("Press: ");
            Console.WriteLine("1 - Player vs AI");
            Console.WriteLine("2 - Player vs Player");
            Console.WriteLine("3 - AI vs AI");
            
            var input = Console.ReadKey();

            switch (input.Key)
            {
                case ConsoleKey.D1:
                    player2.isBot = true;
                    return true;

                case ConsoleKey.D2:
                    return true;

                case ConsoleKey.D3:
                    player1.isBot = true;
                    player2.isBot = true;
                    return true;
            }

            return false;
        }

        private (int, int) CreateFields()
        {
            int fieldWidth = field.GetLength(0);
            int fieldHeight = field.GetLength(1);

            player1.field = (char[,])field.Clone();
            player2.field = (char[,])field.Clone();

            player1.field = GenerateShips(player1.field);
            Thread.Sleep(10);
            player2.field = GenerateShips(player2.field);

            return (fieldHeight, fieldWidth);
        }

        private char[,] GenerateShips(char[,] field)
        {
            Random rnd = new Random();

            int boatCounter = 0;

            for (int i = 0; boatCounter < player1.shipCount; i++)
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

        private void DrawFields(int fieldHeight, int fieldWidth, bool isFirstShow, bool isSecondShow)
        {
            for (int j = 0; j < fieldHeight; j++)
            {
                for (int i = 0; i < fieldWidth; i++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    if (player1.field[i, j] == '■' && !isFirstShow) Console.Write(" "); 
                    else Console.Write(player1.field[i, j]);
                }
                Console.Write("          ");

                for (int i = 0; i < fieldWidth; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (player2.field[i, j] == '■' && !isSecondShow) Console.Write(" ");
                    else Console.Write(player2.field[i, j]);
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(" Player 1 field");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("          Player 2 field");
        }

        private bool CurrentPlayerTurn(Player currentPlayer, Player notCurrentPlayer)
        {
            if (currentPlayer.isBot)
            {
                return TryBotAttack(notCurrentPlayer);               
            }
            else
            {
                return TryPlayerAttack(notCurrentPlayer);
            }
        }

        private void UpdateFields(int fieldHeight, int fieldWidth)
        {
            Console.Clear();
            if (player1.isBot && player2.isBot)
            {
                DrawFields(fieldHeight, fieldWidth, true, true);
            }
            else if (!player1.isBot && player2.isBot)
            {
                DrawFields(fieldHeight, fieldWidth, true, false);
            }
            else if (!player1.isBot && !player2.isBot)
            {
                DrawFields(fieldHeight, fieldWidth, player1.isCurrent, player2.isCurrent);
            }
        }

        private bool TryPlayerAttack(Player notCurrentPlayer)
        {
            if (player1.isCurrent) Console.ForegroundColor = ConsoleColor.DarkBlue;
            else if (player2.isCurrent) Console.ForegroundColor = ConsoleColor.Red;

            int x = CalculateAxis("\n" + "Write the coordinate of X (A, B, C, D, E ... J)", xKeysArray);
            int y = CalculateAxis("\n" + "Write the coordinate of Y (1, 2, 3, 4, 5 ... 10)", yKeysArray) - 1;
            
            if (notCurrentPlayer.field[x, y] is '■')
            {
                notCurrentPlayer.field[x, y] = 'X';
                notCurrentPlayer.shipCount -= 1;
                return true;
            }
            else if (notCurrentPlayer.field[x, y] is ' ')
            {
                notCurrentPlayer.field[x, y] = '·';
                return false;
            }

            return false;
        }

        private bool TryBotAttack(Player notCurrentPlayer)
        {
            Random rnd = new Random();

            Thread.Sleep(rnd.Next(1000, 3000));

            int x = rnd.Next(3, 13);
            int y = rnd.Next(2, 12);

            if (notCurrentPlayer.field[x, y] == '■')
            {
                notCurrentPlayer.field[x, y] = 'X';
                notCurrentPlayer.shipCount -= 1;
                return true;
            }
            else if (notCurrentPlayer.field[x, y] == ' ')
            {
                notCurrentPlayer.field[x, y] = '·';
                return false;
            }

            return false;
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

        private (Player, Player) ChangeCurrentPlayer()
        {
            if (player1.isCurrent)
            {
                player1.isCurrent = false;
                player2.isCurrent = true;
                return (player2, player1);
            }
            else if (player2.isCurrent)
            {
                player1.isCurrent = true;
                player2.isCurrent = false;
                return (player1, player2);
            }

            return (player1, player2);
        }

        private bool isEndGame()
        {
            if (player1.shipCount == 0)
            {
                EndGame("Player 2", ConsoleColor.Red);
                return true;
            }
            else if (player2.shipCount == 0)
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
    }
}