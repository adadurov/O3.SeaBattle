using FluentAssertions;
using NUnit.Framework;
using O3.SeaBattle.Logic;
using System;
using System.Linq;

namespace O3.SeaBattle.Tests.Logic
{
    [TestFixture]
    public class GameInitTest
    {
        [Test]
        public void Init_With_NoShipList_Should_Fail()
        {
            Action c = () => new Game(1, null);
            c.Should().Throw<ArgumentNullException>();
        }

        public void Init_With_NoShips_Should_Fail()
        {
            Action c = () => new Game(1, Enumerable.Empty<Ship>());
            c.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Init_WithZeroSizeField_Should_Fail()
        {
            Action c = () => new Game(0, TestShipFactory.DummyShipList);
            c.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Init_With_Negative_Size_Should_Throw()
        {
            Action c = () => new Game(-1, TestShipFactory.DummyShipList);
            c.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Init_With_Extra_Large_Field_Size_Should_Throw()
        {
            Action c = () => new Game((short)(Game.MaxSize + 1), TestShipFactory.DummyShipList);
            c.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Init_With_Ships_Outside_One_Cell_Range_Should_Fail()
        {
            var distantShip = new[] { new Ship(new Cell(1,1), new Cell(1,1)) };

            Action c = () => new Game(1, distantShip);
            c.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Init_With_Ships_Outside_Range_Should_Fail()
        {
            short distance = 25;
            var distantShip = new[] { new Ship(new Cell(distance, distance), new Cell(distance, distance)) };

            Action c = () => new Game(distance, distantShip);
            c.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Init_With_Intersecting_Ships_Should_Fail_2B()
        {
            var intersectingShips = new[] { 
                TestShipFactory.Create("4D 8H"),
                TestShipFactory.Create("2B 5E")
                };

            Action c = () => new Game(11, intersectingShips);
            c.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Init_With_Intersecting_Ships_Should_Fail_2G()
        {
            var intersectingShips = new[] {
                TestShipFactory.Create("4D 8H"),
                TestShipFactory.Create("2G 5K")
                };

            Action c = () => new Game(11, intersectingShips);
            c.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Init_With_Intersecting_Ships_Should_Fail_7B()
        {
            var intersectingShips = new[] {
                TestShipFactory.Create("4D 8H"),
                TestShipFactory.Create("7B 10E")
                };

            Action c = () => new Game(11, intersectingShips);
            c.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Init_With_Intersecting_Ships_Should_Fail_7G()
        {
            var intersectingShips = new[] {
                TestShipFactory.Create("4D 8H"),
                TestShipFactory.Create("7G 10K")
                };

            Action c = () => new Game(11, intersectingShips);
            c.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Init_With_Intersecting_Ships_Should_Fail_3C()
        {
            var intersectingShips = new[] {
                TestShipFactory.Create("4D 8H"),
                TestShipFactory.Create("3C 10K")
                };

            Action c = () => new Game(11, intersectingShips);
            c.Should().Throw<ArgumentException>();
        }

    }
}
