using System;
using System.Threading;

namespace Sea_Battle
{
    public class Round
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

        private Player player1 = new Player();
        private Player player2 = new Player();

        char[,] firstField;
        char[,] secondField;

        private int fieldWidth;
        private int fieldHeight;

        private int shipCount = 10;

        private bool isHit = false;

        private ConsoleKey[] xKeysArray = { ConsoleKey.A, ConsoleKey.B, ConsoleKey.C, ConsoleKey.D, ConsoleKey.E, ConsoleKey.F, ConsoleKey.G, ConsoleKey.H, ConsoleKey.I, ConsoleKey.J };
        private ConsoleKey[] yKeysArray = { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4, ConsoleKey.D5, ConsoleKey.D6, ConsoleKey.D7, ConsoleKey.D8, ConsoleKey.D9, ConsoleKey.D0 };

        public void RoundLoop(int gameMode, int player1Score, int player2Score)
        {
            if (gameMode == 4) shipCount = 2;

            player1.SetDefaultValues(shipCount, ConsoleColor.DarkBlue, player1Score);
            player2.SetDefaultValues(shipCount, ConsoleColor.Red, player2Score);

            Player currentPlayer = player1;
            Player notCurrentPlayer = player2;

            CreateFields();

            while (!isRoundEnd())
            {
                DrawFields(currentPlayer, gameMode);
                CurrentPlayerTurn(currentPlayer, notCurrentPlayer);
                if (!isHit)
                {
                    (currentPlayer, notCurrentPlayer) = (notCurrentPlayer, currentPlayer);
                    isHit = false;
                }
                Console.Clear();
            }
        }

        private void CreateFields()
        {
            fieldWidth = field.GetLength(0);
            fieldHeight = field.GetLength(1);

            firstField = (char[,])field.Clone();
            secondField = (char[,])field.Clone();

            player1.field.Create(player1);
            Thread.Sleep(10);
            player2.field.Create(player2);
        }

        private void DrawFields(Player currentPlayer, int gameMode)
        {
            Console.Clear();
            for (int j = 0; j < fieldHeight; j++)
            {
                bool isFirstShow = false;
                bool isSecondShow = false;

                switch (gameMode)
                {
                    case 1:
                        isFirstShow = true;
                        isSecondShow = false;
                        break;
                    case 2:
                        isFirstShow = player1 == currentPlayer;
                        isSecondShow = player2 == currentPlayer;
                        break;
                    case 3:
                    case 4:
                        isFirstShow = true;
                        isSecondShow = true;
                        break;
                }
                
                DrawLine(j, isFirstShow, player1, firstField);
                Console.Write("          ");
                DrawLine(j, isSecondShow, player2, secondField);
                Console.WriteLine();
            }

            DrawScore();
        }

        private void DrawScore()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(" Player 1 field");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("          Player 2 field");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"                  {player1.score}:{player2.score}");
        }

        private void DrawLine(int j, bool isShow, Player player, char[,] field)
        {
            int logicJ = j - 2;
            for (int i = 0; i < fieldWidth; i++)
            {
                int logicI = i - 3;

                Console.ForegroundColor = player.color;
                bool isBorder = logicI < 0 || logicJ < 0 || logicI > 9 || logicJ >= 10;

                if (isBorder)
                {
                    Console.Write(field[i, j]);
                }
                else
                {
                    Console.Write(player.field.TryWriteShip(logicI, logicJ, isShow));
                }
            }
        }

        private void CurrentPlayerTurn(Player currentPlayer, Player notCurrentPlayer)
        {
            Console.ForegroundColor = currentPlayer.color;
            if (currentPlayer.isBot)
            {
                TryBotAttack(notCurrentPlayer);
            }
            else
            {
                TryPlayerAttack(notCurrentPlayer);
            }
        }

        private void TryPlayerAttack(Player notCurrentPlayer)
        {
            int x = CalculateAxis("\n" + "Write the coordinate of X (A, B, C, D, E ... J)", xKeysArray);
            int y = CalculateAxis("\n" + "Write the coordinate of Y (1, 2, 3, 4, 5 ... 0)", yKeysArray);

            int newShipCount = notCurrentPlayer.Attack(x, y);          
            if (newShipCount != shipCount)
            {
                isHit = true;
            }
        }

        private void TryBotAttack(Player notCurrentPlayer)
        {
            Random rnd = new Random();

            Thread.Sleep(rnd.Next(1000, 3000));

            int x = rnd.Next(0, 10);
            int y = rnd.Next(0, 10);

            int newShipCount = notCurrentPlayer.Attack(x, y);
            if (newShipCount != shipCount)
            {
                isHit = true;
            }
        }

        private int CalculateAxis(string massage, ConsoleKey[] keysArray)
        {
            Console.WriteLine(massage);

            int input;

            do
            {
                input = Input(keysArray);

                if (input == -1)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Wrong, try again!");
                }

            } while (input == -1);

            return input;
        }

        private int Input(ConsoleKey[] keysArray)
        {
            var input = Console.ReadKey();

            for (int i = 0; i < keysArray.Length; i++)
            {
                if (input.Key == keysArray[i])
                {
                    return i;
                }
            }
            return -1;
        }

        private bool isRoundEnd()
        {
            if (player1.shipCount == 0)
            {
                player2.score += 1;
                DrawMassage("Player 2 won the round!", 1000);
                return true;
            }
            else if (player2.shipCount == 0)
            {
                player1.score += 1;
                DrawMassage("Player 1 won the round!", 1000);
                return true;
            }
            return false;
        }

        public (int, int) GetRoundResult()
        {
            return (player1.score, player2.score);
        }

        private void DrawMassage(string msg, int time)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(msg);
            Thread.Sleep(time);
        }
    }
}