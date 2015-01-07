namespace Baloons_UnitTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using BalloonsPop;


    // All classes and all methods should be tested for correct behavior and for exceptions! 
    // Test largest and smallest possible values!

    [TestClass]
    public class Baloons_Tests
    {
        Balloons testBalloons = BalloonsFactory.SetGameField(GameSize.Small);

        [TestMethod]
        public void FillWithRandomBalloonsShouldReturnFilledArray()
        {
            testBalloons.FillWithRandomBalloons();

            Assert.IsNotNull(testBalloons.Cell[testBalloons.Rows - 1, testBalloons.Columns - 1]);
        }
    }
}
