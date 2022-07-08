using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models
{
    public class ShoppingContext : DbContext
    {
        public ShoppingContext(DbContextOptions<ShoppingContext> options) : base(options)
        {

        }

        public DbSet<Category> category { get; set; }
        public DbSet<Product> product { get; set; }
        public DbSet<Inventory> inventory { get; set; }
        public DbSet<Cart> cart { get; set; }
        public DbSet<Sale> sale { get; set; }
        public DbSet<ProductsSold> productsSold { get; set; }

    }

    public class Category
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        public string desc { get; set; }
    }

    public class Product
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        public string desc { get; set; }
        [ForeignKey("categ")]
        public int categId { get; set; }
        public Category categ { get; set; }
        [Required]
        public float price { get; set; }
        public float tax { get; set; }
        public int count  { get; set; }
        public string img { get; set; }
        [NotMapped]
        public IFormFile imgFile { get; set; }
    }
    public class Inventory
    {
        public int id { get; set; }
        [ForeignKey("prod")]
        public int prodId { get; set; }
        public Product prod { get; set; }
        [Required]
        public int count { get; set; }
        public int reorderLevel { get; set; }

    }

    public class Cart
    {
        public int id { get; set; }
        public string userId { get; set; }
        [ForeignKey("prod")]
        public int prodId { get; set; }
        public Product prod { get; set; }
        public int count { get; set; }
        public float subtotal { get; set; }
        public float tax { get; set; }
        public float total { get; set; }
        public DateTime timeStamp { get; set; }
        public bool check { get; set; }
    }

    public class Sale
    {
        public int id { get; set; }
        public string userId { get; set; }
        public float subtotal { get; set; }
        public float tax { get; set; }
        public float total { get; set; }
        public DateTime timeStamp { get; set; }
    }

    public class ProductsSold
    {
        public int id { get; set; }
        [ForeignKey("prod")]
        public int prodId { get; set; }
        public Product prod { get; set; }
        public int count { get; set; }
        public float price { get; set; }
        public float tax { get; set; }
        public float totalPrice { get; set; }
        public string userId { get; set; }
        public DateTime timeStamp { get; set; }
    }
}
