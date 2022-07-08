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
    [Authorize(Roles = "admin")]
    public class ProductsController : Controller
    {
        private readonly ShoppingContext _context;

        public ProductsController(ShoppingContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var shoppingContext = _context.product.Include(p => p.categ);
            return View(await shoppingContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.product == null)
            {
                return NotFound();
            }

            var product = await _context.product
                .Include(p => p.categ)
                .FirstOrDefaultAsync(m => m.id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["categId"] = new SelectList(_context.category, "id", "name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Product product)
        {
            foreach (Product p in _context.product.ToList())
            {
                if (p.name == product.name)
                {
                    ViewData["categId"] = new SelectList(_context.category, "id", "name");
                    return View(product);
                }
            }
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            
            ViewData["categId"] = new SelectList(_context.category, "id", "name");
            return View(_context.product.Find(id));
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,desc,categId,price,tax,img")] Product product)
        {
            _context.product.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.product == null)
            {
                return NotFound();
            }

            var product = await _context.product
                .Include(p => p.categ)
                .FirstOrDefaultAsync(m => m.id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.product == null)
            {
                return Problem("Entity set 'ShoppingContext.product'  is null.");
            }
            var product = await _context.product.FindAsync(id);
            if (product != null)
            {
                _context.product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.product?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
