namespace BalloonsPop
{
    using System;

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter game field size: small/medium/large");
            string size = Console.ReadLine();
            Balloons newGame = BalloonsFactory.SetGameField(GameSize.Small);

            switch (size)
            {
                case "small":
                    {                                            
                        break;
                    }
                case "medium":
                    {
                        newGame = BalloonsFactory.SetGameField(GameSize.Medium);                        
                        break;
                    }
                case "large":
                    {
                        newGame = BalloonsFactory.SetGameField(GameSize.Large);                        
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid move or command");
                        Main();
                        break;
                    }                          
            }
            newGame.StartGame();
        } 
    }
}