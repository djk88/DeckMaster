using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebSecurity_Day03.Data;
using WebSecurity_Day03.Models;
using WebSecurity_Day03.Repositories;

namespace WebSecurity_Day03.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        // Home page shows list of items.
        // Item price is set through the ViewBag.
        public IActionResult Index()
        {
            if(User != null && User.Identity.IsAuthenticated){
                UserRepo userRepo = new UserRepo(_context);
                MyRegisteredUser user = userRepo.GetUserByEmail(User.Identity.Name);
                HttpContext.Session.SetString("FirstName", user.FirstName);
            }
            return View("Index", "3.55|CAD");
        }

        // Home page shows list of items.
        // Item price is set through the ViewBag.

        [Authorize(Roles = "Manager")]
        public IActionResult Transactions()
        {
            DbSet<IPN> items = _context.IPNs;

            return View(items);
        }

        // This method receives and stores
        // the Paypal transaction details.
        [HttpPost]
        public JsonResult PaySuccess([FromBody] IPN ipn)
        {
            try
            {
                _context.IPNs.Add(ipn);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return Json(ipn);
        }

        //[Authorize]
        //public IActionResult SecureArea()
        //{
        //    // Get user name of user who is logged in.
        //    // This line must be in the controller.
        //    string userName = User.Identity.Name;

        //    // Usually this section would be in a repository.
        //    var registeredUser = _context.MyRegisteredUsers
        //                                 .Where(ru => ru.Email == userName)
        //                                 .FirstOrDefault();  // return one item

        //    return View(registeredUser);
        //}

        // Home page shows list of items.
        // Item price is set through the ViewBag.
        public IActionResult Confirmation(string confirmationId)
        {
            IPN transaction =
            _context.IPNs.FirstOrDefault(t => t.paymentID == confirmationId);

            return View("Confirmation", transaction);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}