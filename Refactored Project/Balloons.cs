namespace BalloonsPop
{
    // Edited - moved "using" statements inside namespace
    // Edited - corrected spelling mistakes in class and method names
    // Edited - corrected code formatting

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    [Author("Shishko Shishkov")] // Edited - applied attribute, instead of comment


    public class Balloons
    {
        public const int Rows = 5; //Edited - wrong names
        public const int Columns = 10;

        private static int remainingCells = Rows * Columns;
        private static int counter = 0;
        private static int clearedCells = 0;
        public static string[,] cell = new string[Rows, Columns]; //Edited: changed public classes to internal
        private static StringBuilder input = new StringBuilder();


        public static void StartGame() // Edited - renamed method
        {
            Console.WriteLine("Welcome to “Balloons Pops” game. Please try to pop the balloons."
                                + "Use 'top' to view the top scoreboard, 'restart' to start a new game and "
                                + "'exit' to quit the game."); // Edited - wrapped string

            remainingCells = Rows * Columns; // Moved here from irrelevant method
            counter = 0; // Moved here from irrelevant method
            clearedCells = 0; // Moved here from irrelevant method

            FillWithRandomBalloons(); // Edited - extracted smaller methods
            RenderGraphics();
            GameLogic(input);
        }


        public static void FillWithRandomBalloons() // Edited - renamed method
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    cell[r, c] = GetRandomChar();
                }
            }
        }


        static Random rand = new Random();

        public static string GetRandomChar() // Edited - changed name
        {
            string legalChars = "1234";
            string randomChar = null;
            randomChar = legalChars[rand.Next(0, legalChars.Length)].ToString();
            return randomChar;
        }


        private static void RenderGraphics() // Edited - refactored the method to allow changing number of cells in the game field
        {
            int[] columnsNumbers = new int[Columns];
            for (int i = 0; i < columnsNumbers.Length; i++)
            {
                columnsNumbers[i] = i;
            }

            string columnsNumbersString = "      " + string.Join(" ", columnsNumbers);
            Console.WriteLine(columnsNumbersString);
            PrintDashesLine(columnsNumbersString);

            for (int r = 0; r < Rows; r++)
            {
                if (r < 10)
                {
                    Console.Write(" ");
                }
                if (r < 100)
                {
                    Console.Write(" ");
                }

                Console.Write(r + " | ");
                for (int c = 0; c < Columns; c++)
                {
                    int columnDigits = (int)Math.Floor(Math.Log10(c + 1) + 1);
                    Console.Write(cell[r, c]);

                    for (int i = 0; i < columnDigits; i++)
                    {
                        Console.Write(" ");
                    }
                }

                Console.WriteLine("| ");
            }

            PrintDashesLine(columnsNumbersString);
        }


        private static void PrintDashesLine(string columnsNumbersString)
        {
            Console.Write("      ");
            for (int i = 0; i < columnsNumbersString.Length - 4; i++)
            {
                Console.Write("-");
            }

            Console.WriteLine();
        }


        public static void GameLogic(StringBuilder userInput)
        {
            PlayGame();
            counter++;
            userInput.Clear();
            GameLogic(userInput);
        }


        private static bool IsLegalMove(int r, int c)
        {
            if ((r < 0) || (c < 0) || (c > Columns - 1) || (r > Rows - 1))
            {
                return false;
            }
            else
            {
                return (cell[r, c] != ".");
            }
        }


        private static void InvalidInputHandler() // Edited - changed method name
        {
            Console.WriteLine("Invalid move or command");
            input.Clear();
            GameLogic(input);
        }


        private static void IllegalMoveHandler() // Edited - changed method name
        {
            Console.WriteLine("Illegal move: cannot pop missing balloon!");
            input.Clear();
            GameLogic(input);
        }


        private static void Exit()
        {
            Console.WriteLine("Good Bye");
            Thread.Sleep(1000);
            Console.WriteLine("Balloons popped: " + counter.ToString());
            Console.WriteLine("Remaining balloons: " + remainingCells.ToString()); // Edited - added descriptive message for user
            Environment.Exit(0);
        }

        // Edited - moved variable before first method to use it
        // Edited: Replaced SortedDictionary with List to allow multiple players with same score in statistics
        private static IList<KeyValuePair<int, string>> statistics = new List<KeyValuePair<int, string>>();

        private static void ReadTheInput()
        {
            if (!IsFinished())
            {
                Console.Write("Enter a row and column: ");
                input.Append(Console.ReadLine());
            }
            else
            {
                Console.Write("Congratulations! You popped all balloons in " + counter + " moves."
                                 + "Please enter your name for the top scoreboard: ");

                var username = Console.ReadLine(); // Edited - same variable was used for two irrelevant functions. Added new variable.				
                var statisticsEntry = new KeyValuePair<int, string>(counter, username);
                statistics.Add(statisticsEntry);
                ShowStatistics();
                input.Clear();
                StartGame();
            }
        }

        // Edited - removed unnecessary methods PrintAgain and Restart

        private static void ShowStatistics()
        {
            int maxPlayersToShow = 4;
            int p = 0;
            Console.WriteLine("Scoreboard:");
            foreach (KeyValuePair<int, string> s in statistics)
            {
                if (p == maxPlayersToShow) break; // Edited - Replaced magic number!
                else
                {
                    p++;
                    Console.WriteLine("{0}. {1} --> {2} moves", p, s.Value, s.Key);
                }
            }
            if (statistics.Count == 0) // Edited - added info message
            {
                Console.WriteLine("No entries yet.");
            }
        }


        private static void PlayGame()
        {
            int r = -1;
            int c = -1;

        Play: ReadTheInput();
            string hop = input.ToString();

            // Edited - Replaced multiple ifs with a switch
            switch (input.ToString())
            {
                case "":
                    {
                        InvalidInputHandler();
                        break;
                    }
                case "top":
                    {
                        ShowStatistics();
                        input.Clear();
                        goto Play;
                    }
                case "restart":
                    {
                        input.Clear();
                        StartGame();
                        break;
                    }
                case "exit":
                    {
                        Exit();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            string activeCell;
            input.Replace(" ", "");

            try
            {
                r = Int32.Parse(input.ToString()) / 10;
                c = Int32.Parse(input.ToString()) % 10;
            }
            catch (Exception)
            {
                InvalidInputHandler();
            }

            if (IsLegalMove(r, c))
            {
                activeCell = cell[r, c];
                Clear(r, c, activeCell);
            }
            else
            {
                IllegalMoveHandler();
            }

            MoveBalloons();
            RenderGraphics();
        }


        private static void Clear(int r, int c, string activeCell)
        {
            if ((r >= 0) && (r <= Rows - 1) && (c <= Columns - 1) && (c >= 0) && (cell[r, c] == activeCell)) // Edited - removed magic numbers
            {
                cell[r, c] = ".";
                clearedCells++;
                //Up
                Clear(r - 1, c, activeCell);
                //Down
                Clear(r + 1, c, activeCell);
                //Left
                Clear(r, c + 1, activeCell);
                //Right
                Clear(r, c - 1, activeCell);
            }
            else
            {
                remainingCells -= clearedCells;
                clearedCells = 0;
                return;
            }
        }


        private static void MoveBalloons() // Edited - renamed method
        {
            int r;
            int c;
            Queue<string> temp = new Queue<string>();

            for (c = Columns - 1; c >= 0; c--)
            {
                for (r = Rows - 1; r >= 0; r--)
                {
                    if (cell[r, c] != ".")
                    {
                        temp.Enqueue(cell[r, c]);
                        cell[r, c] = ".";
                    }
                }

                r = Rows - 1; // Edited - removed magic number
                while (temp.Count > 0)
                {
                    cell[r, c] = temp.Dequeue();
                    r--;
                }

                temp.Clear();
            }
        }

        private static bool IsFinished()
        {
            return (remainingCells == 0);
        }

        static void Main() // Edited - removed unnecessary class
        {
            Balloons.StartGame();
        }
    }

    // Edited - removed unnecessary class RND  
}
