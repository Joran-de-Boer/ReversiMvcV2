using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReversiMvcV2.Models;

namespace ReversiMvcV2.Controllers
{
    public class SpelController : Controller
    {

        //https://localhost:7184/Spel
        // GET: SpelController
        public ActionResult Index()
        {
            Console.WriteLine("hey");
            List<Spel>? spellen = ApiRequester.GetAllSpellen();
            Console.WriteLine(spellen);
            if(spellen is null)
            {
                return View(new List<Spel>());
            }
            
            return View(spellen);
        }

        public ActionResult Play(string guid)
        {
            Spel? spel = ApiRequester.GetSpelByGuid(guid);
            if(spel is null)
            {
                return View();
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
