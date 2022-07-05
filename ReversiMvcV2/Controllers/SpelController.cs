using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReversiMvcV2.Models;
using System.Security.Claims;

namespace ReversiMvcV2.Controllers
{
    public class SpelController : Controller
    {

        //https://localhost:7184/Spel
        // GET: SpelController
        public ActionResult Index()
        {
            string userid;
            using (GetClaimsPrincipal getter = new GetClaimsPrincipal())
            {
                userid = getter.GetUserId(this.User);
            }
            using (Redirecter redirecter = new Redirecter())
            {
                if (userid != null)
                {
                    var game = redirecter.TryRedirectToGame(userid);
                    if (game != null)
                    {
                        return (ActionResult)game;
                    }
                }
            }


            List<Spel>? spellen = ApiRequester.GetAllSpellen();
            if(spellen is null)
            {
                return View(new List<Spel>());
            }
            
            return View(spellen);
        }

        public async Task<IActionResult> Play()
        {
            string guid = (string)Url.ActionContext.RouteData.Values["id"];
            string userid;
            using (GetClaimsPrincipal getter = new GetClaimsPrincipal())
            {
                userid = getter.GetUserId(this.User);
            }
            using (Redirecter redirecter = new Redirecter())
            {
                if (userid != null)
                {
                    var game = redirecter.TryRedirectToGame(userid, guid);
                    if (game != null)
                    {
                        return game;
                    }
                }
            }



            Spel? spel = ApiRequester.GetSpelByGuid(guid);
            if(spel is null)
            {
                return NotFound();
            }

            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (spel.Speler2 is null && spel.Speler1 != currentUserID)
            {
                spel.Speler2 = currentUserID;
                ApiRequester.JoinSpel(currentUserID, guid);
            }

            if(spel.Speler1 != currentUserID && spel.Speler2 != currentUserID)
            {
                return Unauthorized();
            }

            return View(spel);
        }

        // GET: SpelController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SpelController/Create
        public ActionResult Create()
        {
            string userid;
            using (GetClaimsPrincipal getter = new GetClaimsPrincipal())
            {
                userid = getter.GetUserId(this.User);
            }
            using (Redirecter redirecter = new Redirecter())
            {
                if (userid != null)
                {
                    var game = redirecter.TryRedirectToGame(userid);
                    if (game != null)
                    {
                        return (ActionResult)game;
                    }
                }
            }
            return View();
        }

        // POST: SpelController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: SpelController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
