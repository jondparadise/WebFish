using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebFish.API.Services.Responses
{
    public class MoveResponse
    {
        public Guid MoveId { get; set; }
        public bool Success { get; set; } = false;
        public int QueueOrder { get; set; } = -1;
        public double Wait { get; set; } = 0;
    }
}
