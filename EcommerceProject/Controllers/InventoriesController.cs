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
    [Authorize(Roles = "admin,staff")]
    public class InventoriesController : Controller
    {
        private readonly ShoppingContext _context;

        public InventoriesController(ShoppingContext context)
        {
            _context = context;
        }

        // GET: Inventories
        public async Task<IActionResult> Index()
        {
            var shoppingContext = _context.inventory.Include(i => i.prod);
            return View(await shoppingContext.ToListAsync());
        }
        public async Task<IActionResult> Restock()
        {
            List<Inventory> list = new List<Inventory>();
            foreach (Inventory inv in _context.inventory.Include(e=>e.prod).ToList())
            {
                if (inv.count < inv.reorderLevel)
                {
                    list.Add(inv);
                }
            }
            return View(list);
        }

        // GET: Inventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.inventory == null)
            {
                return NotFound();
            }

            var inventory = await _context.inventory
                .Include(i => i.prod)
                .FirstOrDefaultAsync(m => m.id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventories/Create
        public IActionResult Create()
        {
            ViewData["prodId"] = new SelectList(_context.product, "id", "name");
            return View();
        }

        // POST: Inventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,prodId,count,reorderLevel")] Inventory inventory)
        {

            _context.Add(inventory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Inventories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.prodId = new SelectList(_context.product.ToList(), "id", "name");
            return View(_context.inventory.Find(id));
        }

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Inventory inventory)
        {
            _context.inventory.Update(inventory);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Inventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.inventory == null)
            {
                return NotFound();
            }

            var inventory = await _context.inventory
                .Include(i => i.prod)
                .FirstOrDefaultAsync(m => m.id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.inventory == null)
            {
                return Problem("Entity set 'ShoppingContext.inventory'  is null.");
            }
            var inventory = await _context.inventory.FindAsync(id);
            if (inventory != null)
            {
                _context.inventory.Remove(inventory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(int id)
        {
            return (_context.inventory?.Any(e => e.id == id)).GetValueOrDefault();
        }

    }
}
