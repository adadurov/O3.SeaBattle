using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using O3.SeaBattle.Logic;
using System;
using System.Linq;

namespace O3.SeaBattle.Tests.Logic
{
    [TestFixture]
    public class GameInitTest
    {
        private static IMatrixLocationValidator LocationValidator => LocationValidators.Default;

        [Test]
        public void Init_With_NoShipList_Should_Fail()
        {
            Action c = () => new Game(1, null, null);
            c.Should().Throw<ArgumentNullException>();
        }

        public void Init_With_NoShips_Should_Fail()
        {
            Action c = () => new Game(1, Enumerable.Empty<Ship>(), LocationValidator);
            c.Should().Throw<ArgumentException>();
        }

        public void Init_With_No_Validator_Should_Fail()
        {
            Action c = () => new Game(1, Enumerable.Empty<Ship>(), null);
            c.Should().Throw<ArgumentNullException>();
        }


        [Test]
        public void Init_WithZeroSizeField_Should_Fail()
        {
            Action c = () => new Game(0, TestShipFactory.DummyShipList, LocationValidator);
            c.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Init_With_Negative_Size_Should_Throw()
        {
            Action c = () => new Game(-1, TestShipFactory.DummyShipList, LocationValidator);
            c.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Init_With_Extra_Large_Field_Size_Should_Throw()
        {
            Action c = () => new Game((short)(Game.MaxSize + 1), TestShipFactory.DummyShipList, LocationValidator);
            c.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Game_Should_Validate_Ships()
        {
            var distantShip = new[] { new Ship(new Cell(1, 1), new Cell(1, 1)) };

            var validatorMock = Substitute.For<IMatrixLocationValidator>();

            new Game(1, distantShip, validatorMock);

            validatorMock.Received().ValidateShipLocations(1, distantShip);
        }

        [Test]
        public void Init_With_Ships_Outside_One_Cell_Range_Should_Fail()
        {
            var distantShip = new[] { new Ship(new Cell(1,1), new Cell(1,1)) };

            Action c = () => new Game(1, distantShip, LocationValidator);
            c.Should().Throw<ArgumentException>();
        }
    }
}
