using Microsoft.AspNetCore.Mvc;
using ReversiMvcV2.Models;

namespace ReversiMvcV2.Controllers
{
    public class Redirecter : Controller, IDisposable
    {

        private IActionResult RedirectToGame(string id)
        {
            return RedirectToAction("Play", "Spel", new { id });
        }

        public IActionResult? TryRedirectToGame(string playerId)
        {
            return TryRedirectToGame(playerId, null);
        }

        public IActionResult? TryRedirectToGame(string playerId, string? currentGameId)
        {
            Spel? spel = ApiRequester.GetSpelByPlayerId(playerId);
            if (spel is null) { return null; }
            if(spel.ID == currentGameId && currentGameId != null) { return null; }
            return RedirectToGame(spel.ID);
        }

    }
}
