using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebFish.API.Services.Exceptions;
using WebFish.API.Services.Interfaces;
using WebFish.API.Services.Requests;
using WebFish.API.Services.Responses;
using WebFish.Engine;
using WebFish.Engine.Interfaces;

namespace WebFish.API.Services
{
    public class WebFishService : IWebFishService
    {
        private WebFishEngine _publicEngine { get; set; }
        private List<StatusResponse> _queue { get; set; } = new List<StatusResponse>();
        private List<StatusResponse> _complete { get; set; } = new List<StatusResponse>();

        private const string START_FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        public WebFishService()
        {
            this._publicEngine = new WebFishEngine();
            this._publicEngine.CalculationCompleted += _publicEngine_CalculationCompleted;
            this._publicEngine.Initalize();
        }

        private void _publicEngine_CalculationCompleted(Common.CalculationResult result)
        {
            var match = this._queue.FirstOrDefault(item => item.MoveId == result.MoveId && item.Request.GameId == result.GameId);
            if(match != null)
            {
                match.Move = result.MoveFEN;
                match.Completed = true;
                this._queue.Remove(match);
                this._complete.Add(match);
            }
            this.TakeNext();
        }

        private void TakeNext()
        {
            if(this._queue != null && this._queue.Count > 0)
            {
                var selected = this._queue[0];
                this._publicEngine.StartCalculation(new Common.CalculationRequest() { 
                    FEN = selected.FEN,
                    GameId = selected.Request.GameId.Value,
                    MoveId = selected.MoveId
                });
            }
        }

        public async Task<MoveResponse> ProcessMoveRequest(MoveRequest request)
        {
            var moveId = Guid.NewGuid();
            var statusResponse = new StatusResponse()
            {
                Request = request,
                MoveId = moveId,
                FEN = request.FEN,
                Wait = 1
            };

            this._queue.Add(statusResponse);

            var retVal = new MoveResponse() { 
                MoveId = moveId, 
                QueueOrder = this._queue.IndexOf(statusResponse), 
                Success = true, 
                Wait = 1 
            };

            this.TakeNext();

            return retVal;
        }

        public async Task<StatusResponse> GetMoveStatus(StatusRequest request)
        {
            var match = this._queue.FirstOrDefault(item => item.MoveId == request.MoveId && item.Request.GameId == request.GameId);
            if(match != null)
            {
                this.UpdateStatusReponseStatistics(match);
                return match;
            }
            else
            {
                return this._complete.FirstOrDefault(item => item.MoveId == request.MoveId && item.Request.GameId == request.GameId);
            }
        }

        private void UpdateStatusReponseStatistics(StatusResponse response)
        {
            response.QueueOrder = this._queue.IndexOf(response);
        }

        public async Task<List<StatusResponse>> GetQueue()
        {
            List<StatusResponse> retVal = new List<StatusResponse>();
            retVal.AddRange(this._queue);
            retVal.AddRange(this._complete);
            return retVal;
        }
    }
}
