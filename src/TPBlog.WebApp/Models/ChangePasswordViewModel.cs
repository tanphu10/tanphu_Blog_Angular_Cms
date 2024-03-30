using System.ComponentModel.DataAnnotations;
using TPBlog.WebApp.Extensions;

namespace TPBlog.WebApp.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Old password required")]
        public required string OldPassword { get; set; }
        [Required(ErrorMessage = "New password required")]
        public required string NewPassword { get; set; }
        [Required(ErrorMessage = "comfirm new password required")]
        [PasswordMatch("NewPassword", ErrorMessage = "confirm new password is not correct")]
        public required string ConfirmPassword { get; set; }
    }
}
