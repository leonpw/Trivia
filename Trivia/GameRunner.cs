﻿using System;

using UglyTrivia;

namespace Trivia
{
    public class GameRunner
    {
        private static bool currentPlayer;

        public static void Main(String[] args)
        {
            Game aGame = new Game(new[] {"Chet", "Pat", "Sue" });

            int.TryParse(args[0], out int seed);

            Random rand = new Random(seed);

            do
            {
                aGame.Roll(rand.Next(5) + 1);

                if (rand.Next(9) == 7)
                {
                    currentPlayer = aGame.SetPlayerToPenaltyBox();
                }
                else
                {
                    currentPlayer = aGame.IsCorrectAnswer();
                }
            } while (currentPlayer);
        }
    }
}