using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebSecurity_Day03.Data;
using WebSecurity_Day03.Repositories;
using WebSecurity_Day03.ViewModels;

namespace WebSecurity_Day03.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class RoleController : Controller
    {
        ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Role
        public ActionResult Index()
        {
            RoleRepo roleRepo = new RoleRepo(_context);
            return View(roleRepo.GetAllRoles());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleVM role)
        {
            var token = HttpContext.Request.Form["__RequestVerificationToken"];

            RoleRepo roleRepo = new RoleRepo(_context);

            roleRepo.CreateRole(role.RoleName);

            return RedirectToAction("Index", role);

        }

    }
    
}
