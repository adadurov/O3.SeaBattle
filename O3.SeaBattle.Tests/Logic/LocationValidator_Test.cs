using FluentAssertions;
using NUnit.Framework;
using O3.SeaBattle.Logic;
using System;

namespace O3.SeaBattle.Tests.Logic
{
    [TestFixture]
    class LocationValidator_Test
    {
        private static IMatrixLocationValidator LocationValidator => LocationValidators.Default;

        [Test]
        public void Init_With_Ships_Outside_Range_Should_Fail()
        {
            short distance = 25;
            var distantShip = new[] { new Ship(new Cell(distance, distance), new Cell(distance, distance)) };

            Action c = () => LocationValidator.ValidateShipLocations(distance, distantShip);
            c.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Init_With_Intersecting_Ships_Should_Fail_2B()
        {
            var intersectingShips = new[] {
                TestShipFactory.Create("4D 8H"),
                TestShipFactory.Create("2B 5E")
                };

            Action c = () => LocationValidator.ValidateShipLocations(11, intersectingShips);
            c.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Init_With_Intersecting_Ships_Should_Fail_2G()
        {
            var intersectingShips = new[] {
                TestShipFactory.Create("4D 8H"),
                TestShipFactory.Create("2G 5K")
                };

            Action c = () => LocationValidator.ValidateShipLocations(11, intersectingShips);
            c.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Init_With_Intersecting_Ships_Should_Fail_7B()
        {
            var intersectingShips = new[] {
                TestShipFactory.Create("4D 8H"),
                TestShipFactory.Create("7B 10E")
                };

            Action c = () => LocationValidator.ValidateShipLocations(11, intersectingShips);
            c.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Init_With_Intersecting_Ships_Should_Fail_7G()
        {
            var intersectingShips = new[] {
                TestShipFactory.Create("4D 8H"),
                TestShipFactory.Create("7G 10K")
                };

            Action c = () => LocationValidator.ValidateShipLocations(11, intersectingShips);
            c.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Init_With_Intersecting_Ships_Should_Fail_3C()
        {
            var intersectingShips = new[] {
                TestShipFactory.Create("4D 8H"),
                TestShipFactory.Create("3C 10K")
                };

            Action c = () => LocationValidator.ValidateShipLocations(11, intersectingShips);
            c.Should().Throw<ArgumentException>();
        }

    }
}
