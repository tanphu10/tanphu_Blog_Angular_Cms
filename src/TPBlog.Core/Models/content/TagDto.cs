using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;

namespace TPBlog.Core.Models.content
{
    public class TagDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<TagDto, Tag>();
            }
        }
    }
}
