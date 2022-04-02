using System;

namespace WebFish.Common
{
    public class CalculationResult
    {
        public Guid GameId { get; set; }
        public Guid MoveId { get; set; }
        public double RuntimeSeconds { get; set; }
        public string MoveFEN { get; set; }
    }
}
