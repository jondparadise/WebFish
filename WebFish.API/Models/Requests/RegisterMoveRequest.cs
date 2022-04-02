using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebFish.API.Models.Requests
{
    public class RegisterMoveRequest
    {
        public Guid GameId { get; set; }
        public string FEN { get; set; }
    }
}
