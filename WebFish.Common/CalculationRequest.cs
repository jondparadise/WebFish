using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFish.Common
{
    public class CalculationRequest
    {
        public Guid GameId { get; set; }
        public Guid MoveId { get; set; }
        public string FEN { get; set; }
    }
}
