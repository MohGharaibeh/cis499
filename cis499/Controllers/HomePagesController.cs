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
    public class HomePagesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomePagesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: HomePages
        public async Task<IActionResult> Index()
        {
              return _context.HomePages != null ? 
                          View(await _context.HomePages.ToListAsync()) :
                          Problem("Entity set 'ModelContext.HomePages'  is null.");
        }

        // GET: HomePages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HomePages == null)
            {
                return NotFound();
            }

            var homePage = await _context.HomePages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homePage == null)
            {
                return NotFound();
            }

            return View(homePage);
        }

        // GET: HomePages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HomePages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageFile,Description,Text1,Text2,Text3,Text4,Text5")] HomePage homePage)
        {
            if (ModelState.IsValid)
            {
                if (homePage.ImageFile != null && homePage.ImageFile.Length > 0)
                {
                    string w3rootPath = _webHostEnvironment.WebRootPath;

                    string imageName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(homePage.ImageFile.FileName);

                    string path = Path.Combine(w3rootPath , "images", imageName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await homePage.ImageFile.CopyToAsync(fileStream);
                    }
                    homePage.ImagePath = imageName;
                }
                _context.Add(homePage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homePage);
        }

        // GET: HomePages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HomePages == null)
            {
                return NotFound();
            }

            var homePage = await _context.HomePages.FindAsync(id);
            if (homePage == null)
            {
                return NotFound();
            }
            return View(homePage);
        }

        // POST: HomePages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImageFile,Description,Text1,Text2,Text3,Text4,Text5")] HomePage homePage)
        {
            if (id != homePage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (homePage.ImageFile != null && homePage.ImageFile.Length > 0)
                {
                    string w3rootPath = _webHostEnvironment.WebRootPath;

                    string imageName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(homePage.ImageFile.FileName);

                    string path = Path.Combine(w3rootPath , "images", imageName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await homePage.ImageFile.CopyToAsync(fileStream);
                    }
                    homePage.ImagePath = imageName;
                }
                _context.Add(homePage);

                try
                {
                    _context.Update(homePage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomePageExists(homePage.Id))
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
            return View(homePage);
        }

        // GET: HomePages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HomePages == null)
            {
                return NotFound();
            }

            var homePage = await _context.HomePages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homePage == null)
            {
                return NotFound();
            }

            return View(homePage);
        }

        // POST: HomePages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HomePages == null)
            {
                return Problem("Entity set 'ModelContext.HomePages'  is null.");
            }
            var homePage = await _context.HomePages.FindAsync(id);
            if (homePage != null)
            {
                _context.HomePages.Remove(homePage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomePageExists(int id)
        {
          return (_context.HomePages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
