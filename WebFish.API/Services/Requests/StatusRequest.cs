using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebFish.API.Services.Requests
{
    public class StatusRequest
    {
        public Guid GameId { get; set; }
        public Guid MoveId { get; set; }
    }
}
