using EcommerceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProject.Controllers
{
    public class CartController : Controller
    {
        private readonly ShoppingContext _Context;
        private readonly IHttpContextAccessor _contextAccessor;

        public CartController(ShoppingContext context, IHttpContextAccessor accessor)
        {
            _Context = context;
            _contextAccessor = accessor;
        }

        [Authorize]
        public IActionResult Index()
        {
            ViewBag.quantity = 0;
            ViewBag.subtotal = 0;
            ViewBag.tax = 0;
            ViewBag.total = 0;
            List<Cart> list = new List<Cart>();
            foreach (Cart cart in _Context.cart.Include(e => e.prod).ToList())
            {
                if (cart.userId == _contextAccessor.HttpContext.User.Identity.Name)
                {
                    list.Add(cart);
                    ViewBag.subtotal += MathF.Round(cart.subtotal, 2);
                    ViewBag.tax += MathF.Round(cart.tax, 2);
                    ViewBag.total += MathF.Round(cart.total, 2);
                    ViewBag.quantity += cart.count;
                }
            }
            return View(list);
        }

        public async Task<IActionResult> Shopping()
        {
            List<Product> prods = new List<Product>();
            foreach (Product prod in _Context.product.Include(e => e.categ).ToList())
            {
                foreach (Inventory inv in _Context.inventory.ToList())
                {
                    if (prod.id == inv.prodId)
                    {
                        if (inv.count > 0)
                        {
                            prods.Add(prod);
                        }
                    }
                }
            }
            ViewBag.categ = new SelectList(_Context.category.ToList(), "id", "name");
            return View(prods);
        }
        public async Task<IActionResult> ShoppingSearch(Product p)
        {
            List<Product> prods = new List<Product>();
            foreach (Product prod in _Context.product.Include(e => e.categ).ToList())
            {
                foreach (Inventory inv in _Context.inventory.ToList())
                {
                    if (prod.id == inv.prodId)
                    {
                        if (inv.count > 0)
                        {
                            if(p.categId == prod.categId)
                            {
                            prods.Add(prod);
                            }
                        }
                    }
                }
            }
            if (prods.Count == 0)
            {
                Product prod = new Product();
                prod.name = "NoneFound";
                prods.Add(prod);
                ViewBag.categ = new SelectList(_Context.category.ToList(), "id", "name");
                return View(prods);
            }
            ViewBag.categ = new SelectList(_Context.category.ToList(), "id", "name");
            return View(prods);
        }

        [Authorize]
        public async Task<IActionResult> AddToCart(int id)
        {
            ViewBag.count = 0;
            foreach (Inventory i in _Context.inventory.ToList())
            {
                if (i.prodId == id)
                {
                    ViewBag.count = i.count;
                }
            }
            ViewData["categId"] = new SelectList(_Context.category.ToList(), "id", "name");
            Product p = _Context.product.Find(id);
            return View(p);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(Product p)
        {
            Product p2 = _Context.product.Find(p.id);
            foreach (Inventory inventory in _Context.inventory.ToList())
            {
                if (p.id == inventory.prodId)
                {
                    if (inventory.count >= p.count && p.count > 0)
                    {
                        Cart c = new Cart();
                        c.prodId = p.id;
                        c.total = MathF.Round(((p2.price * p2.tax) + p2.price) * p.count, 2);
                        c.subtotal = MathF.Round(p2.price * p.count, 2);
                        c.tax = MathF.Round((p2.tax * p2.price) * p.count, 2);
                        c.count = p.count;
                        c.timeStamp = DateTime.Now;
                        c.userId = _contextAccessor.HttpContext.User.Identity.Name;


                        _Context.cart.Add(c);
                        _Context.SaveChanges();
                        return RedirectToAction("Shopping");

                    }
                }
            }
            ViewBag.count = 0;
            foreach (Inventory i in _Context.inventory.ToList())
            {
                if (i.prodId == p.id)
                {
                    ViewBag.count = i.count;
                }
            }
            ViewData["categId"] = new SelectList(_Context.category.ToList(), "id", "name");
            return View(_Context.product.Find(p.id));
        }

        public async Task<IActionResult> Delete(int id)
        {
            foreach (Cart c in _Context.cart.Include(e => e.prod).ToList())
            {
                if (c.id == id)
                {
                    return View(c);

                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Cart c)
        {
            _Context.cart.Remove(c);
            _Context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteMult()
        {
            List<Cart> list = new List<Cart>();
            foreach (Cart cart in _Context.cart.Include(e => e.prod).ToList())
            {
                if (cart.userId == _contextAccessor.HttpContext.User.Identity.Name)
                {
                    list.Add(cart);
                }
            }
            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteMult(List<Cart> l)
        {
            foreach (Cart c in l)
            {
                if (c.check == true)
                {
                    _Context.cart.Remove(c);
                    _Context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["prodId"] = new SelectList(_Context.product.ToList(), "id", "name");
            return View(_Context.cart.Find(id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Cart c)
        {
            foreach (Inventory i in _Context.inventory.ToList())
            {
                if (i.prodId == c.prodId)
                {
                    if (c.count > 0 && c.count <= i.count)
                    {
                        c.subtotal = MathF.Round(c.prod.price * c.count, 2);
                        c.tax = MathF.Round(c.prod.tax * c.prod.price * c.count, 2);
                        c.total = MathF.Round(c.subtotal + c.tax, 2);
                        _Context.cart.Update(c);
                        _Context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(c);
        }

        public async Task<IActionResult> CheckOut()
        {
            ViewBag.subtotal = 0;
            ViewBag.tax = 0;
            ViewBag.total = 0;
            List<Cart> list = new List<Cart>();
            foreach (Cart cart in _Context.cart.Include(e => e.prod).ToList())
            {
                if (cart.userId == _contextAccessor.HttpContext.User.Identity.Name)
                {
                    list.Add(cart);
                    ViewBag.subtotal += MathF.Round(cart.subtotal, 2);
                    ViewBag.tax += MathF.Round(cart.tax, 2);
                    ViewBag.total += MathF.Round(cart.total, 2);
                }
            }
            Sale sale = new Sale();
            sale.userId = _contextAccessor.HttpContext.User.Identity.Name;
            sale.subtotal = ViewBag.subtotal;
            sale.tax = ViewBag.tax;
            sale.total = ViewBag.total;
            sale.timeStamp = DateTime.Now;

            _Context.sale.Add(sale);
            _Context.SaveChanges();

            foreach (Cart c in list)
            {
                ProductsSold ps = new ProductsSold();
                ps.prodId = c.prodId;
                ps.prod = c.prod;
                ps.count = c.count;
                ps.price = c.subtotal;
                ps.tax = c.tax;
                ps.totalPrice = c.total;
                ps.userId = c.userId;
                ps.timeStamp = DateTime.Now;

                _Context.productsSold.Add(ps);
                _Context.SaveChanges();

                foreach (Inventory inv in _Context.inventory.ToList())
                {
                    if (inv.prodId == c.prodId)
                    {
                        inv.count -= c.count;

                        _Context.inventory.Update(inv);
                        _Context.SaveChanges();
                    }
                }
                _Context.cart.Remove(c);
                _Context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
