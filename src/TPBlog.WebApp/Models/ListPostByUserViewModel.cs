﻿using TPBlog.Core.Models;
using TPBlog.Core.Models.content;

namespace TPBlog.WebApp.Models
{
    public class ListPostByUserViewModel
    {
        public string Keyword { get; set; }
        public int TotalPosts { get; set; }
        public int TotalDraftPosts { get; set; }
        public int TotalWaitingApprovalPosts { get; set; }
        public int TotalPublishedPosts { get; set; }
        public int TotalUnpaidPosts { get; set; }
        public double TotalUnpaidAmount { get; set; }
        public double TotalPaidAmount { get; set; }
        public PageResult<PostInListDto> Posts { get; set; }
    }
}
