using System;
using System.Collections.Generic;
using System.Linq;

namespace UglyTrivia
{
    public class Game
    {
        private const int AmountOfQuestions = 50;
        private List<string> players = new List<string>();

        private int[] places = new int[6];
        private int[] purses = new int[6];

        private bool[] inPenaltyBox = new bool[6];

        private LinkedList<string> popQuestions = new LinkedList<string>();
        private LinkedList<string> scienceQuestions = new LinkedList<string>();
        private LinkedList<string> sportsQuestions = new LinkedList<string>();
        private LinkedList<string> rockQuestions = new LinkedList<string>();

        private int currentPlayer = 0;
        private bool isGettingOutOfPenaltyBox;

        public Game(IEnumerable<string> playerNames)
        {
            AddPlayers(playerNames);
            GenerateQuestions(AmountOfQuestions);
        }

        private void AddPlayers(IEnumerable<string> playerNames)
        {
            foreach (var name in playerNames)
            {
                AddPlayer(name);
            }
        }

        private void GenerateQuestions(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                popQuestions.AddLast(CreateQuestion(i, "Pop"));
                scienceQuestions.AddLast(CreateQuestion(i, "Science"));
                sportsQuestions.AddLast(CreateQuestion(i, "Sports"));
                rockQuestions.AddLast(CreateQuestion(i, "Rock"));
            }
        }

        public String CreateQuestion(int index, string name)
        {
            return $"{name} Question {index}";
        }

        public bool isPlayable()
        {
            return (howManyPlayers() >= 2);
        }

        private bool AddPlayer(string name)
        {
            players.Add(name);
            places[howManyPlayers()] = 0;
            purses[howManyPlayers()] = 0;
            inPenaltyBox[howManyPlayers()] = false;

            Console.WriteLine(name + " was added");
            Console.WriteLine("They are player number " + players.Count);
            return true;
        }

        public int howManyPlayers()
        {
            return players.Count;
        }

        public void roll(int roll)
        {
            Console.WriteLine(players[currentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (inPenaltyBox[currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    isGettingOutOfPenaltyBox = true;

                    Console.WriteLine(players[currentPlayer] + " is getting out of the penalty box");
                    places[currentPlayer] = places[currentPlayer] + roll;
                    if (places[currentPlayer] > 11) places[currentPlayer] = places[currentPlayer] - 12;

                    Console.WriteLine(players[currentPlayer]
                            + "'s new location is "
                            + places[currentPlayer]);
                    Console.WriteLine("The category is " + GetCategory());
                    askQuestion();
                }
                else
                {
                    Console.WriteLine(players[currentPlayer] + " is not getting out of the penalty box");
                    isGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
                places[currentPlayer] = places[currentPlayer] + roll;
                if (places[currentPlayer] > 11) places[currentPlayer] = places[currentPlayer] - 12;

                Console.WriteLine(players[currentPlayer]
                        + "'s new location is "
                        + places[currentPlayer]);
                Console.WriteLine("The category is " + GetCategory());
                askQuestion();
            }
        }

        private void askQuestion()
        {
            if (GetCategory() == "Pop")
            {
                Console.WriteLine(popQuestions.First());
                popQuestions.RemoveFirst();
            }
            if (GetCategory() == "Science")
            {
                Console.WriteLine(scienceQuestions.First());
                scienceQuestions.RemoveFirst();
            }
            if (GetCategory() == "Sports")
            {
                Console.WriteLine(sportsQuestions.First());
                sportsQuestions.RemoveFirst();
            }
            if (GetCategory() == "Rock")
            {
                Console.WriteLine(rockQuestions.First());
                rockQuestions.RemoveFirst();
            }
        }

        private String GetCategory()
        {
            switch (places[currentPlayer])
            {
                case 0:
                case 4:
                case 8:
                    return "Pop";

                case 1:
                case 5:
                case 9:

                    return "Science";

                case 2:
                case 6:
                case 10:

                    return "Sports";

                default:
                    return "Rock";
            }
        }

        public bool IsCorrectAnswer()
        {
            if (inPenaltyBox[currentPlayer])
            {
                if (isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct!!!!");
                    purses[currentPlayer]++;
                    Console.WriteLine(players[currentPlayer]
                            + " now has "
                            + purses[currentPlayer]
                            + " Gold Coins.");

                    bool winner = didPlayerWin();
                    currentPlayer++;
                    if (currentPlayer == players.Count) currentPlayer = 0;

                    return winner;
                }
                else
                {
                    currentPlayer++;
                    if (currentPlayer == players.Count) currentPlayer = 0;
                    return true;
                }
            }
            else
            {
                Console.WriteLine("Answer was corrent!!!!");
                purses[currentPlayer]++;
                Console.WriteLine(players[currentPlayer]
                        + " now has "
                        + purses[currentPlayer]
                        + " Gold Coins.");

                bool winner = didPlayerWin();
                currentPlayer++;
                if (currentPlayer == players.Count) currentPlayer = 0;

                return winner;
            }
        }

        public bool SetPlayerToPenaltyBox()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(players[currentPlayer] + " was sent to the penalty box");
            inPenaltyBox[currentPlayer] = true;

            currentPlayer++;
            if (currentPlayer == players.Count) currentPlayer = 0;
            return true;
        }

        private bool didPlayerWin()
        {
            return !(purses[currentPlayer] == 6);
        }
    }
}