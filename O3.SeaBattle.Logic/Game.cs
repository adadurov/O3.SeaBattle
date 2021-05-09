using Ardalis.GuardClauses;
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

        private int _statShotCount;
        private int _statDestroyedCount;
        private int _statKnockedCount;

        public Game(int size, IEnumerable<Ship> liveShips, IMatrixLocationValidator locationValidator)
        {
            Guard.Against.OutOfRange(size, nameof(size), 1, Game.MaxSize);
            Guard.Against.Null(liveShips, nameof(liveShips));

            if (!liveShips.Any())
            {
                throw new ArgumentException("Cannot start sea battle game without a ship.", nameof(liveShips));
            }

            _locationValidator = locationValidator;

            _locationValidator.ValidateShipLocations(size, liveShips);

            _liveShips = liveShips.ToList();

            _size = size;
            _shotMarks = new ShotMarksMatrix(size, size);
            _statShotCount = 0;
            _statKnockedCount = 0;
            _statDestroyedCount = 0;
            _originalShipCount = _liveShips.Count;
        }

        public GameStats GetStatistics() => new() {
            ShipCount = _originalShipCount,
            Destroyed = _statDestroyedCount,
            Knocked = _statKnockedCount,
            ShotCount = _statShotCount
        };

        public ShotResult Shot(Cell location)
        {
            var validationResult = IsShotValid(location);
            if (validationResult.Faulted)
            {
                return validationResult.ResultToReturn;
            }

            RememberShot(location);

            return MissKnockOrDestroy(location);
        }

        private (bool Faulted, ShotResult ResultToReturn) IsShotValid(Cell location)
        {
            if (Finished)
                return (true, GameAlreadyFinished());

            if (!IsLocationInMatrix(location))
                return (true, ShotResult.InvalidShotLocation);

            if (EverFiredOn(location))
                return (true, ShotResult.DuplicateShot);

            return (false, default(ShotResult));
        }

        public bool IsLocationInMatrix(Cell loc) => _locationValidator.IsWithinMatrix(_size, loc);


        private ShotResult MissKnockOrDestroy(Cell location)
        {
            var targetShip = FindTargetShip(location);

            if (targetShip is null)
            {
                return ShotResult.MissedShot;
            }

            var shotsBefore = targetShip.AddShot();

            if (shotsBefore == 0)
            {
                _statKnockedCount++;
            }

            if (targetShip.IsAlive)
            {
                return new ShotResult { Knocked = true };
            }

            return DestroyShip(targetShip);
        }

        private void RememberShot(Cell shot)
        {
            _shotMarks.AddShotMark(shot);
            _statShotCount++;
        }

        private bool EverFiredOn(Cell newShot) => _shotMarks.EverFiredOn(newShot);

        public bool Finished => ! _liveShips.Any();

        private ShotResult GameAlreadyFinished() => new ShotResult { GameFinished = true };

        private Ship FindTargetShip(Cell newShot)
        {
            return _liveShips.FirstOrDefault(s => s.SpansOver(newShot));
        }

        private ShotResult DestroyShip(Ship knockedShip)
        {
            _liveShips.Remove(knockedShip);
            _statDestroyedCount++;

            var gameFinished = ! _liveShips.Any();

            if (gameFinished)
                return ShotResult.FinalShot;

            return new () 
            {
                Knocked = true,
                Destroyed = true
            };
        }
    }
}
