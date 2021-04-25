using FluentAssertions;
using NUnit.Framework;
using O3.SeaBattle.Logic;

namespace O3.SeaBattle.Tests.Logic
{
    [TestFixture]
    public class GameFinishTest
    {
        [Test]
        public void OneShot_Should_EndGame()
        {
            var g = new Game(
                size: 1,
                new[] { TestShipFactory.A1 },
                LocationValidators.Default
                );

            var shotResult = g.Shot(new Cell(0,0));

            shotResult.GameFinished.Should().Be(true);
            shotResult.Destroyed.Should().Be(true);

            var stat = g.GetStatistics();

            stat.Knocked.Should().Be(1);
            stat.Destroyed.Should().Be(1);
        }
    }
}
