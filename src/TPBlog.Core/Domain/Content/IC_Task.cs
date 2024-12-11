using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TPBlog.Core.Domain.Entity;
using TPBlog.Core.Shared.Enums;

namespace TPBlog.Core.Domain.Content
{
    [Table("IC_Tasks")]
    [Index(nameof(Slug), IsUnique = true)]
    public class IC_Task : EntityBase<Guid>
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
        public DateTimeOffset DueDate { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset Complete { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string ProjectSlug { get; set; }
        public int TimeTrackingSpent { get; set; }
        public int TimeTrackingRemaining { get; set; }
        public int OriginalEstimate { get; set; }
    }
}
