﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;

namespace TPBlog.Core.Models.content
{
    public class CreateUpdateInvtCategoryRequest
    {

        public required string Name { set; get; }

        public required string Slug { set; get; }

        public Guid? ParentId { set; get; }
        public bool IsActive { set; get; }
        public DateTimeOffset DateCreated { set; get; }
        public DateTimeOffset? DateModified { set; get; }

        public string? SeoKeywords { set; get; }

        public string? SeoDescription { set; get; }
        public int SortOrder { set; get; }
        public string ProjectSlug { get; set; }
        public Guid ProjectId { get; set; }
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<CreateUpdateInvtCategoryRequest, IC_InventoryCategory>();
            }
        }
    }
}
