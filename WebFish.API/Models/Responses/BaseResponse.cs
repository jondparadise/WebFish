using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebFish.API.Models.Responses
{
    public abstract class BaseResponse
    {
        public bool Success { get; set; }
    }
}
