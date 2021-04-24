using FluentAssertions;
using NUnit.Framework;
using O3.SeaBattle.Logic;

namespace O3.SeaBattle.Tests
{
    [TestFixture]
    public class GameFinishTest
    {
        [Test]
        public void OneShot_Should_EndGame()
        {
            var g = new Game(
                size: 1,
                new[] { ShipFactory.Create("1A 1A") }
                );

            var shotResult = g.Shot("1A");

            shotResult.GameFinished.Should().Be(true);
            shotResult.Destroyed.Should().Be(true);

            var stat = g.GetStatistics();

            stat.Knocked.Should().Be(1);
            stat.Destroyed.Should().Be(1);
        }
    }
}
