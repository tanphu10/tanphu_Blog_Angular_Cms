using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using AutoMapper;
using TPBlog.Core.Shared.Enums;

namespace TPBlog.Core.Models.content
{
    public class PostDto : PostInListDto
    {

        public Guid CategoryId { get; set; }
        public string? Content { get; set; }
        public Guid? AuthorUserId { get; set; }
        public string? Source { get; set; }
        public string? Tags { get; set; }
        public string? SeoDescription { get; set; }
        public PostStatus Status { get; set; }
      
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<Post, PostDto>();
            }
        }

    }
}
