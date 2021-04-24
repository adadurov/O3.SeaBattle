using System;
using System.Collections.Generic;
using System.Linq;

namespace O3.SeaBattle.Logic
{
    public class Game
    {
        public static int MaxSize => 255;

        private readonly short _size;
        private readonly int _originalShipCount;

        private readonly HashSet<Ship> _liveShips;
        private readonly ShotCache _shotMarks;

        private int _shotCount;
        private int _destroyedCount;
        private int _knockedCount;

        public Game(short size, IEnumerable<Ship> liveShips)
        {
            if (size < 1 || size > Game.MaxSize)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(size),
                    $"The parameter must be greater than 0 and less than or equal to {Game.MaxSize}");
            }

            if (liveShips is null)
            {
                throw new ArgumentNullException(nameof(liveShips));
            }

            if (!liveShips.Any())
            {
                throw new ArgumentException("Cannot start sea battle game without a ship.", nameof(liveShips));
            }

            ValidateShipLocations(size, liveShips);

            _size = size;
            _liveShips = liveShips.ToHashSet();
            _shotMarks = new ShotCache(size, size);
            _shotCount = 0;
            _knockedCount = 0;
            _destroyedCount = 0;
            _originalShipCount = _liveShips.Count;
        }

        public bool IsValidLocation(Location loc) => IsValidLocation(_size, loc);

        private static bool IsValidLocation(short size, Location loc) =>
            loc.Row - Location.Origin.Row >= 0 &&
            loc.Row - Location.Origin.Row < size &&
            loc.Col - Location.Origin.Col >= 0 &&
            loc.Col - Location.Origin.Col < size;

        private static void ValidateShipLocations(short size, IEnumerable<Ship> liveShips)
        {
            var badShipsExist = liveShips.Any(s => !IsValidLocation(size, s.LeftTop) || !IsValidLocation(size, s.RightBottom));

            if (badShipsExist)
            {
                throw new ArgumentException(
                    $"At least one ship is not withint the matrix of {size}x{size}",
                    nameof(liveShips));
            }
        }

        public GameStats GetStatistics() => new() {
            ShipCount = _originalShipCount,
            Destroyed = _destroyedCount,
            Knocked = _knockedCount,
            ShotCount = _shotCount
        };

        public ShotResult Shot(string location) => Shot(LocationFactory.Create(location));

        public ShotResult Shot(Location location)
        {
            if (EverFiredOn(location))
            {
                return ShotResult.DuplicateShot;
            }

            if (!_liveShips.Any())
            {
                return GameEnded();
            }

            RememberShot(location);
            _shotCount++;

            var targetShip = FindTargetShip(location);
            
            if (targetShip is null)
            {
                return ShotResult.MissedShot;
            }

            var shotsBefore = targetShip.AddShot();
            if (shotsBefore == 0)
            {
                _knockedCount++;
            }

            if (targetShip.IsAlive)
            {
                return new ShotResult { Knocked = true };
            }

            return DestroyShip(targetShip);
        }

        private ShotResult GameEnded() => new ShotResult { GameFinished = true };

        private Ship FindTargetShip(Location newShot)
        {
            return _liveShips.FirstOrDefault(s => s.SpansOver(newShot));
        }

        private ShotResult DestroyShip(Ship knockedShip)
        {
            _liveShips.Remove(knockedShip);
            _destroyedCount++;

            var gameFinished = ! _liveShips.Any();

            if (gameFinished)
            {
                return ShotResult.FinalShot;
            }

            return new () 
            {
                Knocked = true,
                Destroyed = true
            };
        }

        private void RememberShot(Location shot) => _shotMarks.AddShotMark(shot);

        private bool EverFiredOn(Location newShot) => _shotMarks.EverFiredOn(newShot);
    }
}
