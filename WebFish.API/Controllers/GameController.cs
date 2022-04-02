using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebFish.API.Views;

namespace WebFish.API.Controllers
{
    
    public class GameController : Controller
    {
        [HttpGet, Route("game")]
        public async Task<IActionResult> Game(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectPermanent($"?id={Guid.NewGuid()}");
            }

            return View("~/Views/GameView.cshtml", new GameViewViewModel(id));
        }
    }
}
