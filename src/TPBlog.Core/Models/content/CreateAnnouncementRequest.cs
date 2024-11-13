using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;

namespace TPBlog.Core.Models.content
{
    public class CreateAnnouncementRequest
    {
        public CreateAnnouncementRequest()
        {
            //AnnouncementUsers = new List<AnnouncementUserViewModel>();
        }
        public int Id { set; get; }

        public string Title { set; get; }

        public string Content { set; get; }

        public Guid? UserId { set; get; }

        public bool Status { get; set; }

        //public virtual ICollection<AnnouncementUserViewModel> AnnouncementUsers { get; set; }
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<CreateAnnouncementRequest, Announcement>();
                CreateMap<AnnouncementUser, AnnouncementUserViewModel>();
            }
        }
    }
}
