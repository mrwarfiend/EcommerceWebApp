using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceProject.Controllers
{
    [Authorize(Roles ="admin")]
    public class CategoriesController : Controller
    {
        private readonly ShoppingContext _context;

        public CategoriesController(ShoppingContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
              return _context.category != null ? 
                          View(await _context.category.ToListAsync()) :
                          Problem("Entity set 'ShoppingContext.category'  is null.");
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.category == null)
            {
                return NotFound();
            }

            var category = await _context.category
                .FirstOrDefaultAsync(m => m.id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,desc")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.category == null)
            {
                return NotFound();
            }

            var category = await _context.category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            _context.category.Update(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.category == null)
            {
                return NotFound();
            }

            var category = await _context.category
                .FirstOrDefaultAsync(m => m.id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.category == null)
            {
                return Problem("Entity set 'ShoppingContext.category'  is null.");
            }
            var category = await _context.category.FindAsync(id);
            if (category != null)
            {
                _context.category.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.category?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
