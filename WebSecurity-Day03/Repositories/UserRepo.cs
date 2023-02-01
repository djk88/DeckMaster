 using WebSecurity_Day03.Data;
using WebSecurity_Day03.Models;
using WebSecurity_Day03.ViewModels;

namespace WebSecurity_Day03.Repositories
{
    public class UserRepo
    {
        ApplicationDbContext _context;

        public UserRepo(ApplicationDbContext context)
        {
            _context = context;

        }

        public List<UserVM> All()
        {
            IEnumerable<UserVM> users = _context.Users.Select(u => new UserVM() { Email = u.Email });

            return users.ToList();
        }

        public MyRegisteredUser GetUserByEmail(string email)
        {
            MyRegisteredUser user = _context.MyRegisteredUsers.Where(x => x.Email == email).FirstOrDefault();

            return user;
        }
    }
}
