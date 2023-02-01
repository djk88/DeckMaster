using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSecurity_Day03.Data;
using WebSecurity_Day03.Models;

namespace WebSecurity_Day03.Controllers
{
    public class ShopController : Controller
    {
        ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            DbSet<Product> product = _context.Product;
            return View(product);
        }
    }
}

