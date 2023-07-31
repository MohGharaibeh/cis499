using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cis499.Models;

namespace cis499.Controllers
{
    public class RequestRecipesController : Controller
    {
        private readonly ModelContext _context;

        public RequestRecipesController(ModelContext context)
        {
            _context = context;
        }

        // GET: RequestRecipes
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.RequestRecipes.Include(r => r.ChefAccount).Include(r => r.UserAccount);
            return View(await modelContext.ToListAsync());
        }

        // GET: RequestRecipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RequestRecipes == null)
            {
                return NotFound();
            }

            var requestRecipe = await _context.RequestRecipes
                .Include(r => r.ChefAccount)
                .Include(r => r.UserAccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requestRecipe == null)
            {
                return NotFound();
            }

            return View(requestRecipe);
        }

        // GET: RequestRecipes/Create
        public IActionResult Create()
        {
            ViewBag.Email = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text2).FirstOrDefault();
            ViewBag.PhoneNumber = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text3).FirstOrDefault();
            ViewBag.Address = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text4).FirstOrDefault();
            ViewData["Chefaccountid"] = new SelectList(_context.Useraccounts.Where(x => x.Roleid == 2), "Id", "FullName");
            ViewData["Useraccountid"] = new SelectList(_context.Useraccounts, "Id", "Id");
            return View();
        }

        // POST: RequestRecipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,State,RecipeName,Ingredient,PreparationTime,Price,Steps,RequestDate,UserAccountId,ChefAccountId")] RequestRecipe requestRecipe)
        {
            if (ModelState.IsValid)
            {
                int? sessionUser = HttpContext.Session.GetInt32("UserId");
                requestRecipe.UserAccountId = sessionUser;
                requestRecipe.State = "Waiting";
                requestRecipe.RequestDate = DateTime.Now;
                requestRecipe.Price = null;
                _context.Add(requestRecipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChefAccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", requestRecipe.ChefAccountId);
            ViewData["UserAccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", requestRecipe.UserAccountId);
            return View(requestRecipe);
        }

        // GET: RequestRecipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RequestRecipes == null)
            {
                return NotFound();
            }

            var requestRecipe = await _context.RequestRecipes.FindAsync(id);
            if (requestRecipe == null)
            {
                return NotFound();
            }
            ViewData["ChefAccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", requestRecipe.ChefAccountId);
            ViewData["UserAccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", requestRecipe.UserAccountId);
            return View(requestRecipe);
        }

        // POST: RequestRecipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,State,RecipeName,Ingredient,PreparationTime,Price,Steps,RequestDate,UserAccountId,ChefAccountId")] RequestRecipe requestRecipe)
        {
            if (id != requestRecipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requestRecipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestRecipeExists(requestRecipe.Id))
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
            ViewData["ChefAccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", requestRecipe.ChefAccountId);
            ViewData["UserAccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", requestRecipe.UserAccountId);
            return View(requestRecipe);
        }

        // GET: RequestRecipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RequestRecipes == null)
            {
                return NotFound();
            }

            var requestRecipe = await _context.RequestRecipes
                .Include(r => r.ChefAccount)
                .Include(r => r.UserAccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requestRecipe == null)
            {
                return NotFound();
            }

            return View(requestRecipe);
        }

        // POST: RequestRecipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RequestRecipes == null)
            {
                return Problem("Entity set 'ModelContext.RequestRecipes'  is null.");
            }
            var requestRecipe = await _context.RequestRecipes.FindAsync(id);
            if (requestRecipe != null)
            {
                _context.RequestRecipes.Remove(requestRecipe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestRecipeExists(int id)
        {
          return (_context.RequestRecipes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
