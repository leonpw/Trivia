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

        private bool AddPlayer(string name)
        {
            players.Add(name);
            places[CountPlayers()] = 0;
            purses[CountPlayers()] = 0;
            inPenaltyBox[CountPlayers()] = false;

            WritePlayersInfo(name);
            return true;
        }

        private void WritePlayersInfo(string name)
        {
            Console.WriteLine(name + " was added");
            Console.WriteLine("They are player number " + players.Count);
        }

        public int CountPlayers()
        {
            return players.Count;
        }

        public void Roll(int roll)
        {
            string playerName = players[currentPlayer];
            Console.WriteLine(playerName + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (inPenaltyBox[currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    isGettingOutOfPenaltyBox = true;
                    WriteInfoPlayerOutOfPenaltyBox(playerName);
                    int place = places[currentPlayer] = SetPlayerBack(roll, places[currentPlayer]);
                    WriteInfoCurrentPLayer(playerName, place, GetCategory());
                    askQuestion();
                }
                else
                {
                    WritePlayerNotGettingOut(playerName);
                    isGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
                int place = places[currentPlayer] = SetPlayerBack(roll, places[currentPlayer]);
                WriteInfoCurrentPLayer(players[currentPlayer], place, GetCategory());
                askQuestion();
            }
        }

        private void WritePlayerNotGettingOut(string playerName)
        {
            Console.WriteLine(playerName + " is not getting out of the penalty box");
        }

        private void WriteInfoCurrentPLayer(string playerName, int playerPlace, string category)
        {
            Console.WriteLine(playerName
                                        + "'s new location is "
                                        + playerPlace);
            Console.WriteLine("The category is " + category);
        }

        private void WriteInfoPlayerOutOfPenaltyBox(string playerName)
        {
            Console.WriteLine(playerName + " is getting out of the penalty box");
        }

        private int SetPlayerBack(int roll, int currentPlace)
        {
            currentPlace += roll;
            if (currentPlace > 11) currentPlace -= 12;

            return currentPlace;
        }

        private void askQuestion()
        {
            switch (GetCategory())
            {
                case "Pop":
                    Console.WriteLine(popQuestions.First());
                    popQuestions.RemoveFirst();
                    break;

                case "Science":
                    Console.WriteLine(scienceQuestions.First());
                    scienceQuestions.RemoveFirst();
                    break;

                case "Sports":
                    Console.WriteLine(sportsQuestions.First());
                    sportsQuestions.RemoveFirst();
                    break;

                case "Rock":
                    Console.WriteLine(rockQuestions.First());
                    rockQuestions.RemoveFirst();

                    break;
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

                    bool winner = DidPlayerWin();
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

                bool winner = DidPlayerWin();
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

        private bool DidPlayerWin()
        {
            return !(purses[currentPlayer] == 6);
        }
    }
}