using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReversiMvcV2.DAL;
using ReversiMvcV2.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace ReversiMvcV2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SpelerContext _context;

        public HomeController(ILogger<HomeController> logger, SpelerContext context)
        {
            _logger = logger;
            _context = context;

        }

        [Authorize]
        public IActionResult Index()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            Speler? speler = _context.Spelers.FirstOrDefault(s => s.Guid == currentUserID);

            if (speler == null)
            {
                _context.Spelers.Add(new Speler(currentUserID, currentUser.FindFirstValue(ClaimTypes.Email).ToString()));
                _context.SaveChanges();
            }

            Spel? spel = ApiRequester.GetSpelByPlayerId(currentUserID);
            if(spel is not null)
            {
                return RedirectToAction("Play", "Spel", new { spel.ID });
            }

            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}