using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebSecurity_Day03.Data;
using WebSecurity_Day03.Models;
using WebSecurity_Day03.Repositories;
using WebSecurity_Day03.ViewModels;

namespace WebSecurity_Day03.Controllers
{
    // This annotation can be used at the class or method level.
    // The annotation could include a comma separated list or different
    // roles.

    [Authorize(Roles = "Admin, Manager")]

    public class UserRoleController : Controller
    {
        private ApplicationDbContext _context;
        private IServiceProvider _serviceProvider;

        public UserRoleController(ApplicationDbContext context,
                                    IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public ActionResult Index()
        {
            UserRepo userRepo = new UserRepo(_context);
            var users = userRepo.All();
            return View(users);
        }

        // Show all roles for a specific user.
        public async Task<IActionResult> Detail(string userName)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_serviceProvider);
            var roles = await userRoleRepo.GetUserRoles(userName);

            UserRepo userRepo = new UserRepo(_context);
            MyRegisteredUser myRegUser = new MyRegisteredUser();

            var user = userRepo.GetUserByEmail(userName);

            ViewBag.FirstName = user.FirstName;
            ViewBag.LastName = user.LastName;
            ViewBag.UserName = userName;
            return View(roles);
        }

        // Present user with ability to assign roles to a user.
        // It gives two drop downs - the first contains the user names with
        // the requested user selected. The second drop down contains all
        // possible roles.
        public ActionResult Create(string userName)
        {
            // Store the email address of the Identity user
            // which is their user name.
            ViewBag.SelectedUser = userName;

            // Build SelectList with role data and store in ViewBag.
            RoleRepo roleRepo = new RoleRepo(_context);
            var roles = roleRepo.GetAllRoles().ToList();

            // There may be a better way but I have always found using the 
            // .NET dropdown lists to be a challenge. Here is a way to make 
            // it work if you can get the data in the proper format. 

            // 1. Preparation for 'Roles' drop down.
            // a) Build a list of SelectListItem objects which have 'Value' and 
            // 'Text' properties. 
            var preRoleList = roles.Select(r =>
                new SelectListItem { Value = r.RoleName, Text = r.RoleName })
                   .ToList();
            // b) Store the SelectListItem objects in a SelectList object 
            // with 'Value' and 'Text' properties set specifically.
            var roleList = new SelectList(preRoleList, "Value", "Text");

            // c) Store the SelectList in a ViewBag.
            ViewBag.RoleSelectList = roleList;

            // 2. Preparation for 'Users' drop down list. 
            // a) Build a list of SelectListItem objects which have 'Value' and 
            // 'Text' properties.
            var userList = _context.Users.ToList();

            // b) Store the SelectListItem objects in a SelectList object 
            // with 'Value' and 'Text' properties set specifically.
            var preUserList = userList.Select(u => new SelectListItem
            { Value = u.Email, Text = u.Email }).ToList();
            SelectList userSelectList = new SelectList(preUserList
                                                      , "Value"
                                                      , "Text");

            // c) Store the SelectList in a ViewBag.
            ViewBag.UserSelectList = userSelectList;
            return View();
        }

        // Assigns role to user.
        [HttpPost]
        public async Task<IActionResult> Create(UserRoleVM userRoleVM)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_serviceProvider);

            if (ModelState.IsValid)
            {
                var addUR = await userRoleRepo.AddUserRole(userRoleVM.Email,
                                                            userRoleVM.Role);
            }
            try
            {
                return RedirectToAction("Detail", "UserRole",
                       new { userName = userRoleVM.Email });
            }
            catch
            {
                return View();
            }
        }
    }
}
