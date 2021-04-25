using System;
using System.Collections.Generic;
using System.Linq;

namespace O3.SeaBattle.Logic
{
    /// <summary>
    /// Implements a sea battle game on a square matrix.
    /// Methods of the class are not thread safe.
    /// </summary>
    public class Game
    {
        public static short MaxSize { get; } = 255;

        private readonly int _size;
        private readonly int _originalShipCount;

        private readonly IMatrixLocationValidator _locationValidator;
        private readonly IList<Ship> _liveShips;
        private readonly ShotMarksMatrix _shotMarks;

        private int _shotCount;
        private int _destroyedCount;
        private int _knockedCount;

        public Game(int size, IEnumerable<Ship> liveShips, IMatrixLocationValidator locationValidator)
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

            _locationValidator = locationValidator;

            _locationValidator.ValidateShipLocations(size, liveShips);

            _liveShips = liveShips.ToList();

            _size = size;
            _shotMarks = new ShotMarksMatrix(size, size);
            _shotCount = 0;
            _knockedCount = 0;
            _destroyedCount = 0;
            _originalShipCount = _liveShips.Count;
        }

        public bool IsLocationInMatrix(Cell loc) => _locationValidator.IsWithinMatrix(_size, loc);

        public GameStats GetStatistics() => new() {
            ShipCount = _originalShipCount,
            Destroyed = _destroyedCount,
            Knocked = _knockedCount,
            ShotCount = _shotCount
        };

        public ShotResult Shot(Cell location)
        {
            if (!IsLocationInMatrix(location))
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

        public bool Finished => ! _liveShips.Any();


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
