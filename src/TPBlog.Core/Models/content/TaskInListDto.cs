using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Shared.Enums;

namespace TPBlog.Core.Models.content
{
    public class TaskInListDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        [Required]
        [Column(TypeName = "varchar(250)")]
        public required string Slug { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public string UserName { get; set; }
        public TaskUserStatus Status { get; set; }
        public PriorityStatus Priority { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateLastModified { get; set; }
        public DateTimeOffset Complete { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string ProjectSlug { get; set; }
        public Guid  ProjectId { get; set; }
        public int TimeTrackingSpent { get; set; }
        public int TimeTrackingRemaining { get; set; }
        public int OriginalEstimate { get; set; }
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<IC_Task, TaskInListDto>();
            }
        }
    }
}
