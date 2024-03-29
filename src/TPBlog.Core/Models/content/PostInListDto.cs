﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using AutoMapper;

namespace TPBlog.Core.Models.content
{
    public class PostInListDto
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Slug { get; set; }
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? Thumbnail { get; set; }

        public int ViewCount { get; set; }
        public DateTime DateCreated { get; set; }
        public required string CategorySlug { get; set; }
        public required string CategoryName { get; set; }
        public string AuthorUserName { get; set; }
        public string AuthorName { get; set; }
        public PostStatus Status { get; set; }
        public bool IsPaid { get; set; }
        public double RoyaltyAmount { get; set; }
        public DateTime? PaidDate { get; set; }

        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<Post, PostInListDto>();
            }
        }

    }
}
