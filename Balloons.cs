using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading; 

// Edited - corrected spelling mistakes in class and method names
// Edited - corrected code formatting

//harasva li vi koda? za vruzka s mene v Twitter sym #shisho33   // TODO - Change with a label
                                                                 // TODO - Add comments if needed


namespace BalloonsPops
{
	public class Balloons       
	{
		const int rows = 5; //Edited - Wrong names
		const int columns = 10;

		private static int cells = rows * columns;
		private static int counter = 0;	
        private static int clearedCells = 0;
        public static string[,] cell = new string[rows, columns];
		public static StringBuilder input = new StringBuilder();
		private static SortedDictionary<int, string> statistics = new SortedDictionary<int, string>();


        public static void Start()
        {
			Console.WriteLine("Welcome to “Balloons Pops” game. Please try to pop the balloons." 
                                + "Use 'top' to view the top scoreboard, 'restart' to start a new game and " 
                                + "'exit' to quit the game."); // Edited - wrapped string

            FillWithRandomBalloons(); // Edited - divided into smaller methods
            RenderGraphics();
            GameLogic(input);
        }

        private static void FillWithRandomBalloons() // Edited - renamed method
        {
            cells = rows * columns;
            counter = 0;
            clearedCells = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    cell[i, j] = RND.GetRandomInt();
                }
            }
        }


        private static void RenderGraphics() // TODO - refactor the code, so that the game field columns can be changed
        {
            Console.WriteLine("    0 1 2 3 4 5 6 7 8 9");
            Console.WriteLine("   ---------------------");

            for (int i = 0; i < rows; i++)
            {
                Console.Write(i + " | ");

                for (int j = 0; j < columns; j++)
                {
                    Console.Write(cell[i, j] + " ");
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


		private static bool IsLegalMove(int i, int j)
		{
			if ((i < 0) || (j < 0) || (j > columns - 1) || (i > rows - 1)) return false;
			else return (cell[i, j] != ".");
		}


		private static void InvalidInputHandler() // Edited - changed method name
		{
			Console.WriteLine("Invalid move or command");
			input.Clear(); 
			GameLogic(input); 
		}


        private static void IllegalMoveHandler() // Edited - changed method name
		{
			Console.WriteLine("Illegal move: cannot pop missing ballon!");
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
				Start();
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
			int i = -1;
			int j = -1;

			Play : ReadTheIput();

			string hop = input.ToString();

            // TODO - Replace with a switch
			if (input.ToString() == "") InvalidInputHandler();
			if (input.ToString() == "top") { ShowStatistics(); input.Clear(); goto Play; }
            if (input.ToString() == "restart") { input.Clear(); Start(); }
			if (input.ToString() == "exit") Exit();

			string activeCell;
			input.Replace(" ", "");

			try
			{
				i = Int32.Parse(input.ToString()) / 10;
				j = Int32.Parse(input.ToString()) % 10;
			}
			catch(Exception) 
			{
				InvalidInputHandler();
			}

            if (IsLegalMove(i, j))
            {
                activeCell = cell[i, j];
                Clear(i, j, activeCell);
            }
            else
            {
                IllegalMoveHandler();
            }
   
            MoveBalloons();
            RenderGraphics();
        }

		private static void Clear(int i, int j, string activeCell)
		{
			if ((i >= 0) && (i <= rows-1) && (j <= columns-1) && (j >= 0) && (cell[i, j] == activeCell)) // Edited - removed magic numbers
			{
				cell[i, j] = ".";
				clearedCells++;
				//Up
				Clear(i-1, j, activeCell);
				//Down
				Clear(i + 1, j, activeCell);
				//Left
				Clear(i, j + 1, activeCell);
				//Right
				Clear(i, j - 1, activeCell);
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
			int i; 
			int j;
			Queue<string> temp = new Queue<string>();

			for (j = columns-1; j >= 0; j--)
			{
				for (i = rows-1; i >= 0; i--)
				{
					if (cell[i, j] != ".")
					{
						temp.Enqueue(cell[i, j]);
						cell[i, j] = ".";
					}
				}

				i = rows-1; // Edited - removed magic number

				while (temp.Count > 0)
				{
					cell[i, j] = temp.Dequeue();
					i--;
				}

				temp.Clear();
			}
		}

		private static bool IsFinished() 
		{
			return (cells == 0);
		}
	}


    public static class RND
    {
        static Random r = new Random();

        public static string GetRandomInt()
        {
            string legalChars = "1234";
            string builder = null;
            builder = legalChars[r.Next(0, legalChars.Length)].ToString();
            return builder;
        }
    }

    // Edited - removed class into separate file
}
