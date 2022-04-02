using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebFish.API.Services.Requests
{
    public class MoveRequest
    {
        public string FEN { get; set; }
        public Guid? GameId { get; set; }
    }
}
