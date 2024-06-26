﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReversiMvcV2.DAL;
using ReversiMvcV2.Models;
using ReversiMvcV2.Models.Request;

namespace ReversiMvcV2.Controllers
{
    //[Authorize(Roles = "Beheerder, mediator")]
    public class SpelersController : Controller
    {
        private readonly SpelerContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public SpelersController(SpelerContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            //_roleManager = roleManager;
        }

        // GET: Spelers
        [Authorize(Roles = "Beheerder, Mediator")]
        public async Task<IActionResult> Index()
        {
              return _context.Spelers != null ? 
                          View(await _context.Spelers.ToListAsync()) :
                          Problem("Entity set 'SpelerContext.Spelers'  is null.");
        }

        // GET: Spelers/Details/5
        [Authorize(Roles = "Beheerder, Mediator")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Spelers == null)
            {
                return NotFound();
            }

            var speler = await _context.Spelers
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (speler == null)
            {
                return NotFound();
            }

            return View(speler);
        }

        // GET: Spelers/Create
        [Authorize(Roles = "Beheerder, Mediator")]
        public IActionResult CreatePage(string guid)
        {
            return View();
        }

        // POST: Spelers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public async Task<IActionResult> Create(string naam)
        {
            Speler speler = new Speler(naam);
            if (ModelState.IsValid)
            {   
                _context.Add(speler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(speler);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetSpeler(string guid)
        //{
        //    if (guid == null || _context.Spelers == null)
        //    {
        //        return NotFound();
        //    }

        //    var speler = await _context.Spelers
        //        .FirstOrDefaultAsync(m => m.Guid == guid);
        //    if (speler == null)
        //    {
        //        RedirectToAction(nameof(Create));
        //    }

        //    return View(speler);
        //}

        // GET: Spelers/Edit/5

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Spelers == null)
            {
                return NotFound();
            }

            var speler = await _context.Spelers.FindAsync(id);
            if (speler == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            if(!roles.Contains("Beheerder") && roles.Contains("Mediator"))
            {
                return RedirectToAction("EditForMediator", "Spelers", new { id });
            }


            return View(speler);
        }

        public async Task<IActionResult> EditForMediator(string id)
        {
            if (id == null || _context.Spelers == null)
            {
                return NotFound();
            }

            var speler = await _context.Spelers.FindAsync(id);
            if (speler == null)
            {
                return NotFound();
            }


            return View(speler);
        }

        // POST: Spelers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Beheerder")]
        
        public async Task<IActionResult> Edit(string id, [Bind("Guid,Naam,AantalGewonnen,AantalVerloren,AantalGelijk,Role")] Speler speler)
        {
            if (id != speler.Guid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(speler);
                    var user = await _userManager.FindByIdAsync(id);
                    var roles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                    await _userManager.AddToRoleAsync(user, speler.Role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpelerExists(speler.Guid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(speler);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Mediator")]

        public async Task<IActionResult> EditForMediator(string id, [Bind("Guid,Naam,AantalGewonnen,AantalVerloren,AantalGelijk")] Speler speler)
        {
            if (id != speler.Guid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(speler);
                    var user = await _userManager.FindByIdAsync(id);
                    var roles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                    await _userManager.AddToRoleAsync(user, speler.Role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpelerExists(speler.Guid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(speler);
        }

        // GET: Spelers/Delete/5
        [Authorize(Roles = "Beheerder, Mediator")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Spelers == null)
            {
                return NotFound();
            }

            var speler = await _context.Spelers
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (speler == null)
            {
                return NotFound();
            }

            return View(speler);
        }

        // POST: Spelers/Delete/5
        [Authorize(Roles = "Beheerder, Mediator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Spelers == null)
            {
                return Problem("Entity set 'SpelerContext.Spelers'  is null.");
            }
            var speler = await _context.Spelers.FindAsync(id);
            if (speler != null)
            {
                _context.Spelers.Remove(speler);
            }

            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
        private bool SpelerExists(string id)
        {
          return (_context.Spelers?.Any(e => e.Guid == id)).GetValueOrDefault();
        }

        //[Authorize(Roles = "Beheerder, Mediator")]
        //public void Ban(string id)
        //{

        //}



        // Spelers/Win
        [HttpPost("Win")]
        public void Win([FromBody] WinRequest request)
        {
            Speler? speler = _context.Spelers.Where(speler => speler.Guid == request.SpelerID).FirstOrDefault();
            if (speler != null)
            {
                switch (request.IsWin)
                {
                    case 0:
                        speler.AantalGelijk += 1;
                        break;
                    case 1:
                        speler.AantalGewonnen += 1;
                        break;
                    case 2:
                        speler.AantalVerloren += 1;
                        break;

                }

                _context.SaveChanges();
            }
        }

            public async Task<IActionResult> Roles(string id)
            {
                
                
                return View();
            }

        public async Task<IActionResult> ScoreBoard()
        {
            return _context.Spelers != null ?
                          View(await _context.Spelers.ToListAsync()) :
                          Problem("Entity set 'SpelerContext.Spelers'  is null.");
        }
    }
}

