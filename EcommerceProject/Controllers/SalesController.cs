using EcommerceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProject.Controllers
{
    [Authorize(Roles ="admin,staff")]
    public class SalesController : Controller
    {
        private readonly ShoppingContext _context;
        public SalesController(ShoppingContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.sale.ToListAsync());
        }

        public async Task<IActionResult> ProductsSold()
        {
            return View(await _context.productsSold.Include(e=>e.prod).ToListAsync());
        }
    }
}
