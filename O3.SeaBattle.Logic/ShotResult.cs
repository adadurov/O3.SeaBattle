namespace O3.SeaBattle.Logic
{
    public class ShotResult
    {
        public bool Knocked { get; init; }

        public bool Destroyed { get; init; }

        public bool GameFinished { get; init; }

        public bool Duplicate { get; init; }

        public bool InvalidLocation { get; init; }

        public bool IsFaulted => Duplicate || InvalidLocation;

        public static ShotResult MissedShot { get; } = new() { };

        public static ShotResult DuplicateShot { get; } = new() { Duplicate = true };

        public static ShotResult InvalidShotLocation { get; } = new() { InvalidLocation = true };

        public static ShotResult FinalShot { get; } = new() {
            Destroyed = true,
            Knocked = true,
            GameFinished = true
        };
    }
}
