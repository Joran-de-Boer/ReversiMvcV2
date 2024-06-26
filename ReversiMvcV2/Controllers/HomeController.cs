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
                var emailprincipal = currentUser.FindFirstValue(ClaimTypes.Email);
                var email = "";


                if(emailprincipal != null)
                {
                    email = emailprincipal.ToString();
                }

                var rolePrincipal = currentUser.FindFirstValue(ClaimTypes.Role);
                var role = "Speler";

                if(rolePrincipal != null)
                {
                    role = rolePrincipal.ToString();
                }

                var newSpeler = new Speler
                {
                    AantalGelijk = 0,
                    AantalGewonnen = 0,
                    AantalVerloren = 0,
                    Naam = email,
                    Guid = currentUserID,
                    Role = role,
                };
                _context.Spelers.Add(newSpeler);
                _context.SaveChanges();
            }

            Spel? spel = ApiRequester.GetSpelByPlayerId(currentUserID);
            if(spel is not null)
            {
                return RedirectToAction("Play", "Spel", new { spel.ID });
            }

            return RedirectToAction("Index", "Spel");
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