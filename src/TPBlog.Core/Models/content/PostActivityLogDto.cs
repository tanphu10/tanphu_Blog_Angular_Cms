using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;

namespace TPBlog.Core.Models.content
{
    public class PostActivityLogDto
    {
        public PostStatus FromStatus { set; get; }

        public PostStatus ToStatus { set; get; }

        public DateTime DateCreated { get; set; }

        public string? Note { set; get; }

        public string UserName { get; set; }

        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<PostActivityLog, PostActivityLogDto>();
            }
        }
    }
}
