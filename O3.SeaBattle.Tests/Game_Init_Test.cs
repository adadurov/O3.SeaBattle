using FluentAssertions;
using NUnit.Framework;
using O3.SeaBattle.Logic;
using System;
using System.Linq;

namespace O3.SeaBattle.Tests
{
    [TestFixture]
    public class GameInitTest
    {
        private static readonly Ship[] DummyShipList = new[] { ShipFactory.Create("1A 1B") };

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
        public void Init_With_TooLargeField_Should_Fail()
        {
            Action c = () => new Game(256, DummyShipList);
            c.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Init_WithZeroSizeField_Should_Fail()
        {
            Action c = () => new Game(0, DummyShipList);
            c.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Init_With_Negative_Size_Should_Throw()
        {
            Action c = () => new Game(-1, DummyShipList);
            c.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Init_With_Ships_Outside_One_Cell_Range_Should_Fail()
        {
            var distantShip = new[] { ShipFactory.Create("2B 2B") };

            Action c = () => new Game(1, distantShip);
            c.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Init_With_Ships_Outside_Range_Should_Fail()
        {
            var distantShip = new[] { ShipFactory.Create("26Z 26Z") };

            Action c = () => new Game(25, distantShip);
            c.Should().Throw<ArgumentException>();
        }

    }
}
