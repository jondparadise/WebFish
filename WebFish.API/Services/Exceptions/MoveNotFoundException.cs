using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebFish.API.Services.Requests;

namespace WebFish.API.Services.Exceptions
{
    public class MoveNotFoundException : Exception
    {
        public StatusRequest Request { get; set; }
    }
}
