namespace BalloonsPops
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    // Edited - moved "using" statements inside namespace
    // Edited - corrected spelling mistakes in class and method names
    // Edited - corrected code formatting

    //harasva li vi koda? za vruzka s mene v Twitter sym #shisho33   // TODO - Change with a label
    // TODO - Add comments if needed
    // TODO - Do we need any additional try-catch blocks with errors?

	public class Balloons       
	{
		const int Rows = 5; //Edited - wrong names
		const int Columns = 10;

		private static int cells = Rows * Columns;
		private static int counter = 0;	
        private static int clearedCells = 0;
        private static string[,] cell = new string[Rows, Columns]; //Edited: changed public classes to private
		private static StringBuilder input = new StringBuilder();
		private static SortedDictionary<int, string> statistics = new SortedDictionary<int, string>();

        public static void StartGame() // Edited - renamed method
        {
			Console.WriteLine("Welcome to “Balloons Pops” game. Please try to pop the balloons." 
                                + "Use 'top' to view the top scoreboard, 'restart' to start a new game and " 
                                + "'exit' to quit the game."); // Edited - wrapped string

            FillWithRandomBalloons(); // Edited - extracted smaller methods
            RenderGraphics();
            GameLogic(input);
        }

        private static void FillWithRandomBalloons() // Edited - renamed method
        {
            cells = Rows * Columns;
            counter = 0;
            clearedCells = 0;

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    cell[r, c] = RND.GetRandomInt();
                }
            }
        }


        private static void RenderGraphics() // TODO - refactor the method, so that the number of Columns in the game field can be changed
        {
            Console.WriteLine("    0 1 2 3 4 5 6 7 8 9");
            Console.WriteLine("   ---------------------");

            for (int r = 0; r < Rows; r++)
            {
                Console.Write(r + " | ");

                for (int c = 0; c < Columns; c++)
                {
                    Console.Write(cell[r, c] + " ");
                }
                Console.Write("| ");
                Console.WriteLine();
            }

            Console.WriteLine("   ---------------------");
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
			Console.WriteLine(cells.ToString());
			Environment.Exit(0);
		}


		private static void ReadTheIput()
		{
			if (!IsFinished())
			{
				Console.Write("Enter a row and column: ");
				input.Append(Console.ReadLine());
			}
			else
			{
				Console.Write("Alriiiiiight! You popped all baloons in "+ counter +" moves."
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
				if (p == 4) break; // TODO - Magic number!
				else
				{
					p++;
					Console.WriteLine("{0}. {1} --> {2} moves",p , s.Value, s.Key);
				}
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
				cells -= clearedCells;
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
			return (cells == 0);
		}


        static void Main() // Edited - removed class
        {
            Balloons.StartGame();
        }  
	}


    public static class RND // TODO - Do we need this class? If we do, it should be in a separate file.
    {
        static Random rand = new Random();

        public static string GetRandomInt()
        {
            string legalChars = "1234";
            string randomNumber = null;
            randomNumber = legalChars[rand.Next(0, legalChars.Length)].ToString();
            return randomNumber;
        }
    }
}
