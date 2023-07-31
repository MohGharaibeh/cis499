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
    public class VirtualbanksController : Controller
    {
        private readonly ModelContext _context;

        public VirtualbanksController(ModelContext context)
        {
            _context = context;
        }

        // GET: Virtualbanks
        public async Task<IActionResult> Index()
        {
              return _context.Virtualbanks != null ? 
                          View(await _context.Virtualbanks.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Virtualbanks'  is null.");
        }

        // GET: Virtualbanks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Virtualbanks == null)
            {
                return NotFound();
            }

            var virtualbank = await _context.Virtualbanks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (virtualbank == null)
            {
                return NotFound();
            }

            return View(virtualbank);
        }

        // GET: Virtualbanks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Virtualbanks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BankName,CardNumber,Balance,Cvv")] Virtualbank virtualbank)
        {
            if (ModelState.IsValid)
            {
                _context.Add(virtualbank);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(virtualbank);
        }

        // GET: Virtualbanks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Virtualbanks == null)
            {
                return NotFound();
            }

            var virtualbank = await _context.Virtualbanks.FindAsync(id);
            if (virtualbank == null)
            {
                return NotFound();
            }
            return View(virtualbank);
        }

        // POST: Virtualbanks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BankName,CardNumber,Balance,Cvv")] Virtualbank virtualbank)
        {
            if (id != virtualbank.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(virtualbank);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VirtualbankExists(virtualbank.Id))
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
            return View(virtualbank);
        }

        // GET: Virtualbanks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Virtualbanks == null)
            {
                return NotFound();
            }

            var virtualbank = await _context.Virtualbanks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (virtualbank == null)
            {
                return NotFound();
            }

            return View(virtualbank);
        }

        // POST: Virtualbanks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Virtualbanks == null)
            {
                return Problem("Entity set 'ModelContext.Virtualbanks'  is null.");
            }
            var virtualbank = await _context.Virtualbanks.FindAsync(id);
            if (virtualbank != null)
            {
                _context.Virtualbanks.Remove(virtualbank);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VirtualbankExists(int id)
        {
          return (_context.Virtualbanks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
