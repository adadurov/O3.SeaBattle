using FluentAssertions;
using NUnit.Framework;
using O3.SeaBattle.Service.Parsers;
using System;

namespace O3.SeaBattle.Tests.Parsers
{
    [TestFixture]
    public class ShipFactoryTest
    {
        private ShipFactory ShipFactory => new ShipFactory(new LocationParser(1, 'A', short.MaxValue));

        [Test]
        public void Create_WithNullSpec_Should_Throw()
        {
            Action c = () => ShipFactory.Create(null);
            c.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Create_WithOneLocation_Should_Throw()
        {
            Action c = () => ShipFactory.Create("Z1");
            c.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Create_WithThreeLocations_Should_Throw()
        {
            Action c = () => ShipFactory.Create("1B 2D 4 E");
            c.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Create_WithBadOrder_Should_Throw()
        {
            Action c = () => ShipFactory.Create("1B 2A");
            c.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Create_WithProperArguments_Should_Be_Live()
        {
            var ship = ShipFactory.Create("1A 2B");
            ship.IsAlive.Should().BeTrue();
        }

        [Test]
        public void Ship_WithOneCell_ShouldBeKilledByOneShot()
        {
            var ship = ShipFactory.Create("1A 1A");

            ship.IsAlive.Should().BeTrue();
            ship.AddShot();
            ship.IsAlive.Should().BeFalse();
        }

        [Test]
        public void Ship_WithFourCells_ShouldBeKilledByFourShots()
        {
            var ship = ShipFactory.Create("1A 4A");

            ship.IsAlive.Should().BeTrue();
            ship.AddShot();
            ship.AddShot();
            ship.AddShot();
            ship.IsAlive.Should().BeTrue();
            ship.AddShot();
            ship.IsAlive.Should().BeFalse();
        }

        [Test]
        public void Create_WithProperArguments_Should_Succeed()
        {
            var ship = ShipFactory.Create("1A 2B");
            ship.Should().NotBeNull();

            ship.LeftTop.Row.Should().Be(0);
            ship.LeftTop.Col.Should().Be(0);

            ship.LeftBottom.Row.Should().Be(1);
            ship.LeftBottom.Col.Should().Be(0);

            ship.RightBottom.Row.Should().Be(1);
            ship.RightBottom.Col.Should().Be(1);

            ship.RightTop.Row.Should().Be(0);
            ship.RightTop.Col.Should().Be(1);
        }
    }
}
