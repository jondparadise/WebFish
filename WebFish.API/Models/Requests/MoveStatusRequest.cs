using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebFish.API.Models.Requests
{
    public class MoveStatusRequest
    {
        public Guid GameId { get; set; }
        public Guid MoveId { get; set; }

    }
}
