using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Domain.Identity;
using TPBlog.Core.Models.system;

namespace TPBlog.Core.Models.content
{
    public class AnnouncementViewModel
    {
        public AnnouncementViewModel()
        {
            //AnnouncementUsers = new List<AnnouncementUserViewModel>();
        }
        public int Id { set; get; }

        public string Title { set; get; }

        public string Content { set; get; }

        public DateTimeOffset DateCreated { get; set; }

        public Guid UserId { set; get; }

        //public UserDto AppUser { get; set; }

        public bool Status { get; set; }

        //public virtual ICollection<AnnouncementUserViewModel> AnnouncementUsers { get; set; }
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<Announcement, AnnouncementViewModel>();
                //CreateMap<AnnouncementUser, AnnouncementUserViewModel>();
            }
        }
    }
}
