using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TPBlog.WebApp.Models
{
    public class LoginViewModel
    {


        [Required(ErrorMessage = "Email name is required")]
        [DisplayName("Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 character")]
        [DisplayName("Password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
