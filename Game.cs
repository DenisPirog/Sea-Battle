using System;
using System.Threading;

namespace Sea_Battle
{
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

        private int gameMode;
        private int numbeOfRounds;
        private int numberOfShips = 10;

        private Player player1 =  new Player();
        private Player player2 = new Player();

        private ConsoleKey[] xKeysArray = { ConsoleKey.A, ConsoleKey.B, ConsoleKey.C, ConsoleKey.D, ConsoleKey.E, ConsoleKey.F, ConsoleKey.G, ConsoleKey.H, ConsoleKey.I, ConsoleKey.J };
        private ConsoleKey[] yKeysArray = { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4, ConsoleKey.D5, ConsoleKey.D6, ConsoleKey.D7, ConsoleKey.D8, ConsoleKey.D9, ConsoleKey.D0 };

        public void GameLoop()
        {
            do Console.Clear();
            while (isGameModeSelected() == false);

            do Console.Clear();
            while (isNumberOfRoundSelected() == false);

            do
            {
                if(gameMode != 4) DrawMassage("Starting new round...", 2000);

                player1.SetDefaultValues(numberOfShips);
                player2.SetDefaultValues(numberOfShips);

                var fieldParams = CreateFields();

                Player currentPlayer = player1;
                Player notCurrentPlayer = player2;

                while (!isRoundEnd())
                {
                    UpdateFields(fieldParams.Item1, fieldParams.Item2, fieldParams.Item3, fieldParams.Item4, currentPlayer);
                    bool isHit = CurrentPlayerTurn(currentPlayer, notCurrentPlayer);
                    if (!isHit)
                    {
                        (currentPlayer, notCurrentPlayer) = (notCurrentPlayer, currentPlayer);
                        if(gameMode == 2)
                        {
                            DrawMassage("Changing player...", 2000);
                        }
                    }
                    Console.Clear();
                }               
            }
            while (!isGameEnd());
        }

        private bool isGameModeSelected()
        {
            Console.WriteLine("Press: ");
            Console.WriteLine("1 - Player vs AI");
            Console.WriteLine("2 - Player vs Player");
            Console.WriteLine("3 - AI vs AI");
            Console.WriteLine("4 - Debug mode");

            var input = Console.ReadKey();

            switch (input.Key)
            {
                case ConsoleKey.D1:
                    player1.isBot = false;
                    player2.isBot = true;
                    gameMode = 1;
                    return true;

                case ConsoleKey.D2:
                    player1.isBot = false;
                    player2.isBot = false;
                    gameMode = 2;
                    return true;

                case ConsoleKey.D3:
                    player1.isBot = true;
                    player2.isBot = true;
                    gameMode = 3;
                    return true;

                case ConsoleKey.D4:
                    player1.isBot = false;
                    player2.isBot = true;
                    gameMode = 4;
                    numberOfShips = 2;
                    return true;
            }

            return false;
        }

        private bool isNumberOfRoundSelected()
        {
            Console.Write("Number of rounds:");

            var input = Console.ReadLine();

            if (int.TryParse(input, out int result))
            {
                numbeOfRounds = int.Parse(input);
                return true;
            }
            return false;
        }

        private (int, int, char[,], char[,]) CreateFields()
        {
            int fieldWidth = field.GetLength(0);
            int fieldHeight = field.GetLength(1);

            player1.color = ConsoleColor.DarkBlue;
            player2.color = ConsoleColor.Red;

            char[,] firstField = (char[,])field.Clone();
            char[,] secondField = (char[,])field.Clone();

            GenerateShips(player1.logicField);
            Thread.Sleep(10);
            GenerateShips(player2.logicField);

            return (fieldHeight, fieldWidth, firstField, secondField);
        }

        private void GenerateShips(char[,] field)
        {   
            Random rnd = new Random();

            int boatCounter = 0;

            for (int i = 0; boatCounter < player1.shipCount; i++)
            {
                int randomX = rnd.Next(0, 10);
                int randomY = rnd.Next(0, 10);

                if (field[randomX, randomY] == ' ')
                {
                    field[randomX, randomY] = '■';
                    boatCounter++;
                }
            }
        }

        private void DrawFields(int fieldHeight, int fieldWidth, char[,] firstField, char[,] secondField, bool isFirstShow, bool isSecondShow)
        {
            for (int j = 0; j < fieldHeight; j++)
            {
                DrawLine(j, fieldWidth, firstField, isFirstShow, player1);
                Console.Write("          ");
                DrawLine(j, fieldWidth, secondField, isSecondShow, player2);
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(" Player 1 field");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("          Player 2 field");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"                  { player1.score}:{player2.score}");
        }

        private void DrawLine(int j, int fieldWidth, char[,] field, bool isShow, Player player)
        {
            int logicJ = j - 2;
            for (int i = 0; i < fieldWidth; i++)
            {
                int logicI = i - 3;
                DrawChar(logicI, logicJ, i, j, field, isShow, player);
            }
        }

        private void DrawChar(int logicI, int logicJ, int i, int j, char[,] field, bool isShow, Player player)
        {
            Console.ForegroundColor = player.color;
            bool isBound = logicI < 0 || logicJ < 0 || logicI > 9 || logicJ >= 10;

            if (isBound) Console.Write(field[i, j]);
            else Console.Write(player.TryWriteShip(logicI, logicJ, isShow));
        }

        private bool CurrentPlayerTurn(Player currentPlayer, Player notCurrentPlayer)
        {
            Console.ForegroundColor = currentPlayer.color;
            if (currentPlayer.isBot)
            {
                return TryBotAttack(notCurrentPlayer);               
            }
            else
            {
                return TryPlayerAttack(notCurrentPlayer);
            }
        }

        private void UpdateFields(int fieldHeight, int fieldWidth, char[,] firstField, char[,] secondField, Player currentPlayer)
        {
            Console.Clear();
            if (gameMode == 1)
            {
                DrawFields(fieldHeight, fieldWidth, firstField, secondField, true, false);
            }
            else if (gameMode == 2)
            {
                DrawFields(fieldHeight, fieldWidth, firstField, secondField, player1 == currentPlayer, player2 == currentPlayer);               
            }
            else if (gameMode == 3)
            {
                DrawFields(fieldHeight, fieldWidth, firstField, secondField, true, true);
            }
            else if (gameMode == 4)
            {
                DrawFields(fieldHeight, fieldWidth, firstField, secondField, true, true);
            }
        }

        private bool TryPlayerAttack(Player notCurrentPlayer)
        {           
            int x = CalculateAxis("\n" + "Write the coordinate of X (A, B, C, D, E ... J)", xKeysArray);
            int y = CalculateAxis("\n" + "Write the coordinate of Y (1, 2, 3, 4, 5 ... 0)", yKeysArray);

            return notCurrentPlayer.Attack(x, y);
        }

        private bool TryBotAttack(Player notCurrentPlayer)
        {
            Random rnd = new Random();

            Thread.Sleep(rnd.Next(1000, 3000));

            int x = rnd.Next(0, 10);
            int y = rnd.Next(0, 10);

            return notCurrentPlayer.Attack(x, y);
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

            } while (input == - 1);

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
                player2.score += RoundResult("Player 2");
                return true;
            }
            else if (player2.shipCount == 0)
            {
                player1.score += RoundResult("Player 1");
                return true;
            }
            return false;
        }

        private int RoundResult(string winner)
        {
            DrawMassage(winner + " won the round!", 1000);
            return 1;
        }      

        private bool isGameEnd()
        {
            if (player1.score == numbeOfRounds)
            {
                GameResult(player1.color, "Player 1");
                return true;
            }
            else if (player2.score == numbeOfRounds)
            {
                GameResult(player2.color, "Player 2");
                return true;
            }
            return false;
        }

        private void GameResult(ConsoleColor color, string winner)
        {
            Console.Clear();
            Console.ForegroundColor = color;
            Console.WriteLine(winner + " wins!");
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