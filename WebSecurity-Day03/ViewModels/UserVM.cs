using System.ComponentModel.DataAnnotations;

namespace WebSecurity_Day03.ViewModels
{
    public class UserVM
    {
        [Key]
        public string Email { get; set; }
    }
}
