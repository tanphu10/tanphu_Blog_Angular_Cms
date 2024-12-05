using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Identity;

namespace TPBlog.Core.Domain.Content
{
    [Table("IC_AnnouncementUsers")]
    //[Table("IC_Transactions")]
    public class IC_AnnouncementUser
    {
        [Key]
        [Column(Order = 1)]
        public int AnnouncementId { get; set; }  // Foreign Key to Announcement

        [Key]
        [Column(Order = 2)]
        public Guid UserId { get; set; }  // Foreign Key to AppUser

        public bool HasRead { get; set; }

        //public virtual AppUser AppUser { get; set; }

        //public virtual Announcement Announcement { get; set; }
    }
}
