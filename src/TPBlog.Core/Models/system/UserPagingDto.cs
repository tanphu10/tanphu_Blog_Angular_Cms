using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBlog.Core.Models.system
{
    public class UserPagingDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public IList<string> Roles { get; set; }
        public DateTime? Dob { get; set; }
        public string? Avatar { get; set; }
        public string[] RoleClaims { get; set; }
    }
}
