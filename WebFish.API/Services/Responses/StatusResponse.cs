using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebFish.API.Services.Requests;

namespace WebFish.API.Services.Responses
{
    public class StatusResponse
    {
        public MoveRequest Request { get; set; }
        public Guid MoveId { get; set; }
        public bool Completed { get; set; }
        public int QueueOrder { get; set; } = -1;
        public double Wait { get; set; } = 0;
        public string FEN { get; set; } = null;
        public string Move { get; set; } = null;
    }
}
