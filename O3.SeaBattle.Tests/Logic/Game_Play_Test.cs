using FluentAssertions;
using NUnit.Framework;
using O3.SeaBattle.Logic;
using System.Collections.Generic;

namespace O3.SeaBattle.Tests.Logic
{
    [TestFixture]
    public class GamePlayTest
    {
        [Test]
        public void GameStat_Should_BeEmpty()
        {
            var g = new Game(
                size: 2,
                new[] { new Ship(new Cell(0, 0), new Cell(0, 1)) },
                LocationValidators.Default
                );

            var stat0 = g.GetStatistics();

            stat0.ShipCount.Should().Be(1);
            stat0.ShotCount.Should().Be(0);
            stat0.Knocked.Should().Be(0);
            stat0.Destroyed.Should().Be(0);
        }

        [Test]
        public void Fire_SameLocation_ShouldReturn_DuplicateResult()
        {
            var g = new Game(
                size: 2,
                new[] { new Ship(new Cell(0,0), new Cell(0,1)) },
                LocationValidators.Default
                );

            var stat0 = g.GetStatistics();
            stat0.Knocked.Should().Be(0);

            g.Shot(new Cell(0,0));
            var dupShotResult = g.Shot(new Cell(0,0));

            dupShotResult.Duplicate.Should().Be(true);
            dupShotResult.Knocked.Should().Be(false);
            dupShotResult.Destroyed.Should().Be(false);
            dupShotResult.GameFinished.Should().Be(false);
        }

        [Test]
        public void DirectShot_ShouldUpdateStats_KnockedCount()
        {
            var g = new Game(
                size: 2,
                new[] { new Ship(new Cell(0,0), new Cell(0,1)) },
                LocationValidators.Default
                );

            var stat0 = g.GetStatistics();
            stat0.Knocked.Should().Be(0);

            g.Shot(new Cell(0,0));

            var stat1 = g.GetStatistics();
            stat1.Knocked.Should().Be(1);
        }

        [Test]
        public void Shot_Out_Of_Row_Range_Should_Fail()
        {
            var g = new Game(
                size: 2,
                new[] { new Ship(new Cell(0, 0), new Cell(0, 1)) },
                LocationValidators.Default
                );

            var outOfRowRangeResult = g.Shot(new Cell(2, 0));
            outOfRowRangeResult.InvalidLocation.Should().BeTrue();
        }

        [Test]
        public void Shot_Out_Of_Col_Range_Should_Fail()
        {
            var g = new Game(
                size: 2,
                new[] { new Ship(new Cell(0, 0), new Cell(0, 1)) },
                LocationValidators.Default
                );

            var outOfColRangeResult = g.Shot(new Cell(0, 2));
            outOfColRangeResult.InvalidLocation.Should().BeTrue();
        }


        [Test]
        public void FatalShot_ShouldUpdateStats_DestroyedCount()
        {
            var g = new Game(
                size: 2,
                new[] { new Ship(new Cell(0, 0), new Cell(0, 1)) },
                LocationValidators.Default
                );

            var stat0 = g.GetStatistics();
            stat0.Knocked.Should().Be(0);

            g.Shot(new Cell(0,0));
            g.Shot(new Cell(0,1));

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
                    new Ship(new Cell(0, 0), new Cell(0, 1)),
                    new Ship(new Cell(3, 0), new Cell(3, 1))
                },
                LocationValidators.Default
            );

            var stat0 = g.GetStatistics();
            stat0.Knocked.Should().Be(0);

            g.Shot(new Cell(0, 0)); g.Shot(new Cell(0, 1));

            g.Shot(new Cell(3, 0)); g.Shot(new Cell(3, 1));
            
            var stat1 = g.GetStatistics();
            stat1.Knocked.Should().Be(2);
            stat1.Destroyed.Should().Be(2);
        }

    }
}
