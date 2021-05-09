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
        private readonly HashSet<Ship> _liveShips;
        private readonly ShotMarksMatrix _shotMarks;

        private readonly StatisticsCache _statsCache;

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

            _liveShips = liveShips.ToHashSet();

            _size = size;
            _shotMarks = new ShotMarksMatrix(size, size);

            _statsCache = new StatisticsCache();

            _originalShipCount = _liveShips.Count;
        }

        public GameStats GetStatistics()
        {
            if (_statsCache.IsValid)
            {
                return _statsCache.GetValue();
            }

            var destroyedShipCount = _originalShipCount - _liveShips.Count;

            return _statsCache.Update( new()
            {
                ShipCount = _originalShipCount,
                Destroyed = destroyedShipCount,
                Knocked = _liveShips.Count(s => s.IsKnocked) + destroyedShipCount,
                ShotCount = _shotMarks.MarksCount
            });
        }

        public ShotResult Shot(Cell location)
        {
            var validationResult = IsShotValid(location);
            if (!validationResult.ShotValid)
            {
                return validationResult.InvalidShotDetails;
            }

            InvalidateStatistics();

            RememberShot(location);

            return GetShotResultAt(location);
        }

        private void InvalidateStatistics() => _statsCache.Invalidate();

        private (bool ShotValid, ShotResult InvalidShotDetails) IsShotValid(Cell location)
        {
            if (Finished)
                return (false, GameAlreadyFinished());

            if (!IsLocationInMatrix(location))
                return (false, ShotResult.InvalidShotLocation);

            if (EverFiredOn(location))
                return (false, ShotResult.DuplicateShot);

            return (true, default(ShotResult));
        }

        public bool IsLocationInMatrix(Cell loc) => _locationValidator.IsWithinMatrix(_size, loc);


        private ShotResult GetShotResultAt(Cell location)
        {
            var targetShip = FindTargetShip(location);

            if (targetShip is null)
            {
                return ShotResult.MissedShot;
            }

            var hitResult = targetShip.Hit();

            if (hitResult == ShipHitResult.Knocked)
            {
                return KnockShip();
            }

            return DestroyShip(targetShip);
        }

        private void RememberShot(Cell shot)
        {
            _shotMarks.AddShotMark(shot);
        }

        private bool EverFiredOn(Cell newShot) => _shotMarks.EverFiredOn(newShot);

        public bool Finished => ! _liveShips.Any();

        private ShotResult GameAlreadyFinished() => new ShotResult { GameFinished = true };

        private Ship FindTargetShip(Cell newShot)
        {
            return _liveShips.FirstOrDefault(s => s.SpansOver(newShot));
        }

        private ShotResult KnockShip()
        {
            return new ShotResult { Knocked = true };
        }

        private ShotResult DestroyShip(Ship knockedShip)
        {
            _liveShips.Remove(knockedShip);

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
    }
}
