using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TPBlog.WebApp.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email name is required")]
        [DisplayName("Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 character")]
        [DisplayName("Password")]
        public string Password { get; set; }
    }
}
