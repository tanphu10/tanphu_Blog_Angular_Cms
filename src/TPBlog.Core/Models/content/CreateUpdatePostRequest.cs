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
    public class CreateUpdatePostRequest
    {

        public required string Name { get; set; }

        public required string Slug { get; set; }
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? Thumbnail { get; set; }
        public string? FilePdf { get; set; }
        public Guid CategoryId { get; set; }
        public string? Content { get; set; }
        public string? Source { get; set; }
        public string[] Tags { get; set; }
        public string? SeoDescription { get; set; }
        public Guid ProjectId { get; set; }
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<CreateUpdatePostRequest, IC_Post>();
            }
        }

    }
}
