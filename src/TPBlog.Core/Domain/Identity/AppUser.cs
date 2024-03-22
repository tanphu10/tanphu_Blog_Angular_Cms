using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPBlog.Core.Domain.Identity
{
    //[Table("AppUsers")]
    //nếu như chúng ta không để bảng table thì nó sẽ tự generate ra 

    public class AppUser : IdentityUser<Guid>
    {
        [required]
        [MaxLength(100)]
        public required string FirstName { get; set; }
        [required]
        [MaxLength(100)]
        public required string LastName { get; set; }
        public bool IsActive { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTOkenExpiryTime { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? Dob { get; set; }
        [MaxLength(500)]
        public string? Avatar { get; set; }
        public DateTime? VipStartDate { get; set; }
        public DateTime? VipExpireDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public double Balance { get; set; }
        public double RoyaltyAmountPerPost { get; set; }
        public string GetFullName()
        {
            return this.FirstName + " " + this.LastName;
        }
    }
}
