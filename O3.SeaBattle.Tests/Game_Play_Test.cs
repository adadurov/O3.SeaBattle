using FluentAssertions;
using NUnit.Framework;
using O3.SeaBattle.Logic;
using System;

namespace O3.SeaBattle.Tests
{
    [TestFixture]
    public class GamePlayTest
    {
        [Test]
        public void GameStat_Should_BeEmpty()
        {
            var g = new Game(
                size: 2,
                new[] { ShipFactory.Create("1A 1B") }
                );

            var stat0 = g.GetStatistics();

            stat0.ShipCount.Should().Be(1);
            stat0.ShotCount.Should().Be(0);
            stat0.Knocked.Should().Be(0);
            stat0.Destroyed.Should().Be(0);
        }

        [Test]
        public void DirectShot_ShouldUpdateStats_KnockedCount()
        {
            var g = new Game(
                size: 2,
                new[] { ShipFactory.Create("1A 1B") }
                );

            var stat0 = g.GetStatistics();
            stat0.Knocked.Should().Be(0);

            g.Shot("1A");

            var stat1 = g.GetStatistics();
            stat1.Knocked.Should().Be(1);
        }

        [Test]
        public void FatalShot_ShouldUpdateStats_DestroyedCount()
        {
            var g = new Game(
                size: 2,
                new[] {
                    ShipFactory.Create("1A 1B"),
                }
                );

            var stat0 = g.GetStatistics();
            stat0.Knocked.Should().Be(0);

            g.Shot("1A");
            g.Shot("1B");

            var stat1 = g.GetStatistics();
            stat1.Knocked.Should().Be(1);
            stat1.Destroyed.Should().Be(1);
        }

        [Test]
        public void FatalShot_ShouldUpdateStats_DestroyedCount_2()
        {
            var g = new Game(
                size: 4,
                new[] { 
                    ShipFactory.Create("1A 1B"),
                    ShipFactory.Create("4A 4B"),
                }
            );

            var stat0 = g.GetStatistics();
            stat0.Knocked.Should().Be(0);

            g.Shot("1A");
            g.Shot("1B");

            g.Shot("4A");
            g.Shot("4B");

            var stat1 = g.GetStatistics();
            stat1.Knocked.Should().Be(2);
            stat1.Destroyed.Should().Be(2);
        }

    }
}
