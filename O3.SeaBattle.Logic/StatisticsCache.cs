using System;

namespace O3.SeaBattle.Logic
{
    public class StatisticsCache
    {
        private bool _isValid = false;
        private GameStats _stats = null;

        public void Invalidate()
        {
            _isValid = false;
            _stats = null;
        }

        public bool IsValid => _isValid;

        public GameStats GetValue()
        {
            if (!_isValid)
            {
                throw new InvalidOperationException("The cache contains no value.");
            }
            return _stats;
        }

        public GameStats Update(GameStats stats) 
        {
            _stats = stats;
            _isValid = true;
            return stats;
        }
    }
}
