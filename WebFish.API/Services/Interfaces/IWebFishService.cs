using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebFish.API.Services.Requests;
using WebFish.API.Services.Responses;

namespace WebFish.API.Services.Interfaces
{
    public interface IWebFishService
    {
        Task<MoveResponse> ProcessMoveRequest(MoveRequest request);
        Task<StatusResponse> GetMoveStatus(StatusRequest request);

        Task<List<StatusResponse>> GetQueue();
    }
}
