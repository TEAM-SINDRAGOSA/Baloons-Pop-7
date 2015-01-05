﻿namespace BalloonsPop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    // Edited - moved "using" statements inside namespace
    // Edited - corrected spelling mistakes in class and method names
    // Edited - corrected code formatting

    [Author("shisho33")] // Edited - applied attribute, instead of comment
        
        // TODO - Add docuemntation

	public class Balloons       
	{
		public const int Rows = 5; //Edited - wrong names
		public const int Columns = 15;

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

        internal static void FillWithRandomBalloons() // Edited - renamed method
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    cell[r, c] = RND.GetRandomChar();
                }
            }
        }


        private static void RenderGraphics() // Edited - refactored the method, so that the number of Columns in the game field can be changed
        {
            int[] columnsNumbers = new int[Columns];

            for (int i = 0; i < columnsNumbers.Length; i++)
            {
                columnsNumbers[i] = i+1;
            }

            string columnsNumbersString = "    " + string.Join(" ", columnsNumbers);
            Console.WriteLine(columnsNumbersString);

            PrintDashesLine(columnsNumbersString);

            for (int r = 0; r < Rows; r++)
            {
                Console.Write(r + " | ");

                for (int c = 0; c < Columns; c++)
                {
                    int columnDigits = (int)Math.Floor(Math.Log10(c+1) + 1);
                    Console.Write(cell[r, c]);

                    for (int i = 0; i < columnDigits; i++)
                    {
                        Console.Write(" ");   
                    }                        
                }

                Console.Write("| ");
                Console.WriteLine();
            }

            PrintDashesLine(columnsNumbersString);
        }

        private static void PrintDashesLine(string columnsNumbersString)
        {
            Console.Write("    ");

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
			Console.WriteLine(counter.ToString());
			Console.WriteLine(remainingCells.ToString());
			Environment.Exit(0);
		}

        //Edited - moved variable before first method to use it
        private static IDictionary<int, string> statistics = new SortedDictionary<int, string>(); // Edited: Replaced SortedDictionary with IDicionary


		private static void ReadTheIput()
		{
			if (!IsFinished())
			{
				Console.Write("Enter a row and column: ");
				input.Append(Console.ReadLine());
			}
			else
			{
				Console.Write("Congratulations! You popped all baloons in "+ counter +" moves."
								 +"Please enter your name for the top scoreboard:");

                var username = Console.ReadLine(); // Edited - same variable was used for two irrelevant functions. Added new variable.				
				statistics.Add(counter, username);
				ShowStatistics();
				input.Clear();
				StartGame();
			}
		}

		private static void ShowStatistics() // Edited - removed unnecessary methods PrintAgain and Restart
		{
			int p = 0;
			Console.WriteLine("Scoreboard:");
			foreach(KeyValuePair<int, string> s in statistics)
			{
				if (p == 4) break; // TODO - Replace magic number!
				else
				{
					p++;
					Console.WriteLine("{0}. {1} --> {2} moves",p , s.Value, s.Key);
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

			Play : ReadTheIput();

			string hop = input.ToString();

            // TODO - Replace with a switch
			if (input.ToString() == "") InvalidInputHandler();
			if (input.ToString() == "top") { ShowStatistics(); input.Clear(); goto Play; }
            if (input.ToString() == "restart") { input.Clear(); StartGame(); }
			if (input.ToString() == "exit") Exit();

			string activeCell;
			input.Replace(" ", "");

			try
			{
				r = Int32.Parse(input.ToString()) / 10;
				c = Int32.Parse(input.ToString()) % 10;
			}
			catch(Exception) 
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
			if ((r >= 0) && (r <= Rows-1) && (c <= Columns-1) && (c >= 0) && (cell[r, c] == activeCell)) // Edited - removed magic numbers
			{
				cell[r, c] = ".";
				clearedCells++;
				//Up
				Clear(r-1, c, activeCell);
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

			for (c = Columns-1; c >= 0; c--)
			{
				for (r = Rows-1; r >= 0; r--)
				{
					if (cell[r, c] != ".")
					{
						temp.Enqueue(cell[r, c]);
						cell[r, c] = ".";
					}
				}

				r = Rows-1; // Edited - removed magic number

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


        static void Main() // Edited - removed class
        {
            Balloons.StartGame();
        }  
	}


    public static class RND // TODO - Do we need this class or should we put the method in Blaoons class? If we do, it should be in a separate file.
    {
        static Random rand = new Random();

        public static string GetRandomChar() // Edited - changed name
        {
            string legalChars = "1234";
            string randomChar = null;
            randomChar = legalChars[rand.Next(0, legalChars.Length)].ToString();
            return randomChar;
        }
    }
}
