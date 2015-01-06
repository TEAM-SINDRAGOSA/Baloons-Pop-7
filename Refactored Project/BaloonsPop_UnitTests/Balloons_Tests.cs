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
        public void FillWithRandomBalloonsShouldReturnFilledArray()
        {
            Balloons.FillWithRandomBalloons();

            Assert.IsNotNull(Balloons.cell[Balloons.Rows-1,Balloons.Columns-1]);
        }
    }
}
