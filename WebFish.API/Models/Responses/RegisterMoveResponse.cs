using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebFish.API.Services.Responses;

namespace WebFish.API.Models.Responses
{
    public class RegisterMoveResponse : BaseResponse
    {
        public MoveResponse ServiceMoveResponse { get; set; }
        
        public RegisterMoveResponse(MoveResponse serviceResponse)
        {
            this.ServiceMoveResponse = serviceResponse;
        }
    }
}
