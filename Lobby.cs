using System;

namespace Sea_Battle
{
    class Lobby
    {
        private int player1Score;
        private int player2Score;

        private int gameMode;
        private int numberOfRounds;

        Round newRound;

        public void Start()
        {
            Intro();           

            do
            {
                StartNewRound();
                GetRoundResult();

            } while (!isGameEnd());
           
        }

        private void StartNewRound()
        {
            newRound = new Round();
            newRound.RoundLoop(gameMode, player1Score, player2Score);
        }

        private void GetRoundResult()
        {
            (player1Score, player2Score) = newRound.GetRoundResult();
        }

        private void Intro()
        {
            do
            {
                Console.Clear();
            }
            while (isGameModeSelected() == false);

            do
            {
                Console.Clear();
            }
            while (isNumberOfRoundSelected() == false);
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
                    gameMode = 1;
                    return true;

                case ConsoleKey.D2:
                    gameMode = 2;
                    return true;

                case ConsoleKey.D3:
                    gameMode = 3;
                    return true;

                case ConsoleKey.D4:
                    gameMode = 4;
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
                numberOfRounds = int.Parse(input);
                return true;
            }
            return false;
        }

        private bool isGameEnd()
        {
            if (player1Score == numberOfRounds)
            {
                GameResult("Player 1", ConsoleColor.DarkBlue);
                return true;
            }
            else if (player2Score == numberOfRounds)
            {
                GameResult("Player 2", ConsoleColor.Red);
                return true;
            }

            return false;
        }

        private void GameResult(string winner, ConsoleColor color)
        {
            Console.Clear();
            Console.ForegroundColor = color;
            Console.WriteLine(winner + " wins!");
        }
    }
}
