namespace BalloonsPop
{
    using System;

    public static class BalloonsFactory
    {
        public static Balloons SetGameField(GameSize size)
        {
            switch (size)
            {
                case GameSize.Small:
                    return new Balloons(4, 8);
                case GameSize.Medium:
                    return new Balloons(8, 12);
                case GameSize.Large:
                    return new Balloons(10, 14);                    
                default:
                    throw new ArgumentException();
            }
        }
    }
}
