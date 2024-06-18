using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, SpelerContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;

        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Speler? speler = _context.Spelers.FirstOrDefault(s => s.Guid == currentUserID);

            var user = await _userManager.FindByIdAsync(currentUserID);
            var roles = await _userManager.GetRolesAsync(user);

            if (speler == null)
            {
                var newSpeler = new Speler
                {
                    AantalGelijk = 0,
                    AantalGewonnen = 0,
                    AantalVerloren = 0,
                    Naam = currentUser.FindFirstValue(ClaimTypes.Email).ToString(),
                    Guid = currentUserID,
                    Role = currentUser.FindFirstValue(ClaimTypes.Role).ToString(),
                };
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