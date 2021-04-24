using System;
using System.Collections.Generic;
using System.Linq;

namespace O3.SeaBattle.Logic
{
    public class Game
    {
        public static short MaxSize { get; } = 255;

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

            _liveShips = ConvertAndValidateShipLocations(size, liveShips);

            _size = size;
            _shotMarks = new ShotCache(size, size);
            _shotCount = 0;
            _knockedCount = 0;
            _destroyedCount = 0;
            _originalShipCount = _liveShips.Count;
        }

        public bool IsLocationInRange(Cell loc) => IsInRange(_size, loc);

        private static bool IsInRange(short size, Cell loc) =>
            loc.Row >= 0 &&
            loc.Row < size &&
            loc.Col >= 0 &&
            loc.Col < size;

        private static HashSet<Ship> ConvertAndValidateShipLocations(short size, IEnumerable<Ship> liveShips)
        {
            var hashSet = new HashSet<Ship>(liveShips.Count());

            foreach (var candidate in liveShips)
            {
                var isOutOfRange =  !IsInRange(size, candidate.LeftTop) || !IsInRange(size, candidate.RightBottom);

                if (isOutOfRange)
                {
                    throw new ArgumentException(
                        $"At least one ship ({candidate}) is not entirely within the matrix of {size}x{size}.",
                        nameof(liveShips));
                }

                if (hashSet.Any(s => s != candidate && candidate.Overlaps(s)))
                {
                    throw new ArgumentException(
                        $"At least one ship overlaps {candidate}.",
                        nameof(liveShips));
                }

                hashSet.Add(candidate);
            }
            return hashSet;
        }

        public GameStats GetStatistics() => new() {
            ShipCount = _originalShipCount,
            Destroyed = _destroyedCount,
            Knocked = _knockedCount,
            ShotCount = _shotCount
        };

        public ShotResult Shot(Cell location)
        {
            if (!IsLocationInRange(location))
                return ShotResult.InvalidShotLocation;
            if (EverFiredOn(location))
                return ShotResult.DuplicateShot;
            if (!_liveShips.Any())
                return GameAlreadyFinished();

            RememberShot(location);
            _shotCount++;

            var targetShip = FindTargetShip(location);
            
            if (targetShip is null)
                return ShotResult.MissedShot;

            var shotsBefore = targetShip.AddShot();

            if (shotsBefore == 0)
                _knockedCount++;

            if (targetShip.IsAlive)
                return new ShotResult { Knocked = true };

            return DestroyShip(targetShip);
        }

        private ShotResult GameAlreadyFinished() => new ShotResult { GameFinished = true };

        private Ship FindTargetShip(Cell newShot)
        {
            return _liveShips.FirstOrDefault(s => s.SpansOver(newShot));
        }

        private ShotResult DestroyShip(Ship knockedShip)
        {
            _liveShips.Remove(knockedShip);
            _destroyedCount++;

            var gameFinished = ! _liveShips.Any();

            if (gameFinished)
                return ShotResult.FinalShot;

            return new () 
            {
                Knocked = true,
                Destroyed = true
            };
        }

        private void RememberShot(Cell shot) => _shotMarks.AddShotMark(shot);

        private bool EverFiredOn(Cell newShot) => _shotMarks.EverFiredOn(newShot);
    }
}
