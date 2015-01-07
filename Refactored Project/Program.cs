namespace BalloonsPop
{
    using System;

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter game field size: small/medium/large");
            string size = Console.ReadLine();
            

            switch (size)
            {
                case "small":
                    {
                        Balloons newGame = BalloonsFactory.SetGameField(GameSize.Small);
                        newGame.StartGame();
                        break;
                    }
                case "medium":
                    {
                        Balloons newGame = BalloonsFactory.SetGameField(GameSize.Medium);
                        newGame.StartGame();
                        break;
                    }
                case "large":
                    {
                        Balloons newGame = BalloonsFactory.SetGameField(GameSize.Large);
                        newGame.StartGame();
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid move or command");
                        Main();
                        break;
                    }                          
            }

            
        }       

    }
}