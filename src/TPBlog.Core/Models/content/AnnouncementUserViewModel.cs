using TPBlog.Core.Domain.Identity;
using TPBlog.Core.Models.system;

namespace TPBlog.Core.Models.content
{
    public class AnnouncementUserViewModel
    {
        public int AnnouncementId { get; set; }

        public Guid UserId { get; set; }

        public bool HasRead { get; set; }
        public Guid? ProjectId { get; set; }


        public virtual UserDto AppUser { get; set; }

        public virtual AnnouncementViewModel Announcement { get; set; }
    }
}
