using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebFish.API.Models.Requests;
using WebFish.API.Services.Interfaces;
using WebFish.API.Services.Requests;
using WebFish.Engine;
using WebFish.Engine.Interfaces;

namespace WebFish.API.Controllers
{
    [Route("webfish")]
    [ApiController]
    public class WebFishController : ControllerBase
    {
        private readonly IWebFishService _webFish;
        public WebFishController(IWebFishService webFishService)
        {
            this._webFish = webFishService;
        }

        [HttpPost, Route("move")]
        public async Task<IActionResult> RegisterMove([FromBody] RegisterMoveRequest request)
        {
            if (ModelState.IsValid)
            {
                return Ok(await this._webFish.ProcessMoveRequest(new MoveRequest() { FEN = request.FEN, GameId = request.GameId }));
            }

            return BadRequest();
        }

        [HttpGet, Route("move")]
        public async Task<IActionResult> GetMoveStatus(string gameId, string moveId)
        {
            if (ModelState.IsValid)
            {
                return Ok(await this._webFish.GetMoveStatus(new StatusRequest() { GameId = new Guid(gameId), MoveId = new Guid(moveId) }));
            }

            return BadRequest();
        }

        [HttpGet, Route("moves")]
        public async Task<IActionResult> GetMoveQueue()
        {
            return Ok(await this._webFish.GetQueue());
        }
    }
}
