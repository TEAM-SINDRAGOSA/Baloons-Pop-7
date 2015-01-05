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
        [TestMethod]
        public void FillWithRandomBalloonsShouldReturnCorrectArraySize()
        {
            Balloons.StartGame();

            int numberOfCells = Balloons.Rows * Balloons.Columns;
            int length = Balloons.cell.Length;

            Assert.AreEqual(numberOfCells,
                length,
                string.Format("The number of cells should be {0}, but instead is {1}", numberOfCells, length));
        }
    }
}
