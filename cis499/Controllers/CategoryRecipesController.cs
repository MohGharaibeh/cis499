using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cis499.Models;
using Microsoft.Extensions.Hosting;

namespace cis499.Controllers
{
    public class CategoryRecipesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CategoryRecipesController(ModelContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: CategoryRecipes
        public async Task<IActionResult> Index()
        {
              return _context.CategoryRecipes != null ? 
                          View(await _context.CategoryRecipes.ToListAsync()) :
                          Problem("Entity set 'ModelContext.CategoryRecipes'  is null.");
        }

        // GET: CategoryRecipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CategoryRecipes == null)
            {
                return NotFound();
            }

            var categoryRecipe = await _context.CategoryRecipes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryRecipe == null)
            {
                return NotFound();
            }

            return View(categoryRecipe);
        }

        // GET: CategoryRecipes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoryRecipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageFile,Name,Description")] CategoryRecipe categoryRecipe)
        {
            if (ModelState.IsValid)
            {
                string w3Root = _hostEnvironment.WebRootPath;
                string imageName = Guid.NewGuid().ToString() + "_" + categoryRecipe.ImageFile.FileName;
                string path = Path.Combine(w3Root, "/images/" + imageName);
                //using(var fileStream = new FileStream(path, FileMode.Create))
                //{
                //    await useraccount.ImageFile.CopyToAsync(fileStream);
                //}
                categoryRecipe.ImagePath = imageName;
                _context.Add(categoryRecipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryRecipe);
        }

        // GET: CategoryRecipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CategoryRecipes == null)
            {
                return NotFound();
            }

            var categoryRecipe = await _context.CategoryRecipes.FindAsync(id);
            if (categoryRecipe == null)
            {
                return NotFound();
            }
            return View(categoryRecipe);
        }

        // POST: CategoryRecipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImageFile,Name,Description")] CategoryRecipe categoryRecipe)
        {
            if (id != categoryRecipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string w3Root = _hostEnvironment.WebRootPath;
                string imageName = Guid.NewGuid().ToString() + "_" + categoryRecipe.ImageFile.FileName;
                string path = Path.Combine(w3Root, "/images/" + imageName);
                //using(var fileStream = new FileStream(path, FileMode.Create))
                //{
                //    await useraccount.ImageFile.CopyToAsync(fileStream);
                //}
                categoryRecipe.ImagePath = imageName;
                try
                {
                    _context.Update(categoryRecipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryRecipeExists(categoryRecipe.Id))
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
            return View(categoryRecipe);
        }

        // GET: CategoryRecipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CategoryRecipes == null)
            {
                return NotFound();
            }

            var categoryRecipe = await _context.CategoryRecipes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryRecipe == null)
            {
                return NotFound();
            }

            return View(categoryRecipe);
        }

        // POST: CategoryRecipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CategoryRecipes == null)
            {
                return Problem("Entity set 'ModelContext.CategoryRecipes'  is null.");
            }
            var categoryRecipe = await _context.CategoryRecipes.FindAsync(id);
            if (categoryRecipe != null)
            {
                _context.CategoryRecipes.Remove(categoryRecipe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryRecipeExists(int id)
        {
          return (_context.CategoryRecipes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
