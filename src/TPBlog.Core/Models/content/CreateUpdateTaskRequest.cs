using AutoMapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TPBlog.Core.Shared.Enums;
using TPBlog.Core.Domain.Content;

namespace TPBlog.Core.Models.content
{
    public class CreateUpdateTaskRequest
    {
        [Required]
        [MaxLength(250)]
        public required string Name { get; set; }
        [Required]
        [Column(TypeName = "varchar(250)")]
        public required string Slug { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public Guid UserId { get; set; }
        public TaskUserStatus Status { get; set; }
        public PriorityStatus Priority { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public DateTimeOffset Complete { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string ProjectSlug { get; set; }
        public int TimeTrackingSpent { get; set; }
        //public int TimeTrackingRemaining { get; set; }
        public int OriginalEstimate { get; set; }
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<CreateUpdateTaskRequest, IC_Task>();
            }
        }
    }
}
