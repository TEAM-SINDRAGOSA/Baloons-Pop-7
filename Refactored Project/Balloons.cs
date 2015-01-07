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
        private static int counter = 0;
        private static int clearedCells = 0;
        private static StringBuilder input = new StringBuilder();

        public Balloons(int rows,int columns)
        {
            this.Rows = rows;
            this.Columns = columns;
            this.RemainingCells = rows * columns;
            this.Cell = new string[rows, columns];
        }

        public int Rows { get; set; }
        public int Columns { get; set; }
        public int RemainingCells { get; set; }
        public string[,] Cell { get; set; }


        public void StartGame() // Edited - renamed method
        {
            Console.WriteLine("Welcome to “Balloons Pops” game. Please try to pop the balloons."
                                + "Use 'top' to view the top scoreboard, 'restart' to start a new game and "
                                + "'exit' to quit the game."); // Edited - wrapped string

            counter = 0; // Moved here from irrelevant method
            clearedCells = 0; // Moved here from irrelevant method

            FillWithRandomBalloons(); // Edited - extracted smaller methods
            RenderGraphics();
            GameLogic(input);            
        }

        public void FillWithRandomBalloons() // Edited - renamed method
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    Cell[r, c] = GetRandomChar();
                }
            }
        }

        static Random rand = new Random();

        public static string GetRandomChar() // edited - changed name
        {
            string legalchars = "1234";
            string randomchar = null;
            randomchar = legalchars[rand.Next(0, legalchars.Length)].ToString();
            return randomchar;
        }

        private void RenderGraphics() // Edited - refactored the method to allow changing number of cells in the game field
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
                    Console.Write(Cell[r, c]);

                    for (int i = 0; i < columnDigits; i++)
                    {
                        Console.Write(" ");
                    }
                }

                Console.WriteLine("| ");
            }

            PrintDashesLine(columnsNumbersString);
        }

        private void PrintDashesLine(string columnsNumbersString)
        {
            Console.Write("      ");
            for (int i = 0; i < columnsNumbersString.Length - 4; i++)
            {
                Console.Write("-");
            }

            Console.WriteLine();
        }

        public void GameLogic(StringBuilder userInput)
        {
            PlayGame();
            counter++;
            userInput.Clear();
            GameLogic(userInput);
        }

        private bool IsLegalMove(int r, int c)
        {
            if ((r < 0) || (c < 0) || (c > Columns - 1) || (r > Rows - 1))
            {
                return false;
            }
            else
            {
                return (Cell[r, c] != ".");
            }
        }

        private void InvalidInputHandler() // Edited - changed method name
        {
            Console.WriteLine("Invalid move or command");
            input.Clear();
            GameLogic(input);
        }

        private void IllegalMoveHandler() // Edited - changed method name
        {
            Console.WriteLine("Illegal move: cannot pop missing balloon!");
            input.Clear();
            GameLogic(input);
        }

        private void Exit()
        {
            Console.WriteLine("Good Bye");
            Thread.Sleep(1000);
            Console.WriteLine("Moves made: " + counter.ToString());
            Console.WriteLine("Remaining balloons: " + RemainingCells.ToString()); // Edited - added descriptive message for user
            Environment.Exit(0);
        }
        // Edited - moved variable before first method to use it
        // Edited: Replaced SortedDictionary with List to allow multiple players with same score in statistics
        private static IList<KeyValuePair<int, string>> statistics = new List<KeyValuePair<int, string>>();

        private void ReadTheInput()
        {
            if (RemainingCells != 0)
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

        private void PlayGame()
        {
            int r = -1;
            int c = -1;

        Play: ReadTheInput();
            // Edited - removed unnecessary variable
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
        

            ParseInput(ref r, ref c); // Edited - extracted method

            string activeCell;
            if (IsLegalMove(r, c))
            {
                activeCell = Cell[r, c];
                Clear(r, c, activeCell);
            }
            else
            {
                IllegalMoveHandler();
            }

            MoveBalloons();
            RenderGraphics();
        }

        private void ParseInput(ref int r, ref int c)
        {
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
        }

        private void Clear(int r, int c, string activeCell)
        {
            if ((r >= 0) && (r <= Rows - 1) && (c <= Columns - 1) && (c >= 0) && (Cell[r, c] == activeCell)) // Edited - removed magic numbers
            {
                Cell[r, c] = ".";
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
                RemainingCells -= clearedCells;
                clearedCells = 0;
                return;
            }
        }

        private void MoveBalloons() // Edited - renamed method
        {
            int r;
            int c;
            Queue<string> temp = new Queue<string>();

            for (c = Columns - 1; c >= 0; c--)
            {
                for (r = Rows - 1; r >= 0; r--)
                {
                    if (Cell[r, c] != ".")
                    {
                        temp.Enqueue(Cell[r, c]);
                        Cell[r, c] = ".";
                    }
                }

                r = Rows - 1; // Edited - removed magic number
                while (temp.Count > 0)
                {
                    Cell[r, c] = temp.Dequeue();
                    r--;
                }

                temp.Clear();
            }
        }
    }
}