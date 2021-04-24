using FluentAssertions;
using NUnit.Framework;
using O3.SeaBattle.Logic;
using System.Collections.Generic;

namespace O3.SeaBattle.Tests
{
    [TestFixture]
    public class GameplayLongTests
    {
        [Test]
        public void Large_Field_Should_Succeed()
        {
            const byte GameSize = 255;
            short step = 4;

            List<Ship> CreateShips(int size)
            {
                var ships = new List<Ship>(size / step * size);

                for (short r = 0; r < size; r++)
                {
                    short offset = (short)(step / 2 * (r % 2));
                    for (short c = offset; c < size; c += step)
                    {
                        var loc = new Cell(r, c);
                        ships.Add(new Ship(loc, loc));
                    }
                }
                return ships;
            }

            IEnumerable<Cell> GetShots(int size)
            {
                for (short r = 0; r < size; r++)
                {
                    for (short c = 0; c < size; c++)
                    {
                        yield return new Cell(size - 1 - r, size - 1 - c);
                    }
                }
                yield break;
            }

            var ships = CreateShips(GameSize);
            var largeGame = new Game(GameSize, ships);

            largeGame.GetStatistics().ShipCount.Should().Be(ships.Count);

            bool gameFinished = false;

            foreach (var shotLocation in GetShots(GameSize))
            {
                var result = largeGame.Shot(shotLocation);
                if (result.GameFinished)
                {
                    gameFinished = true;
                }
            }

            gameFinished.Should().BeTrue();
        }
    }
}
