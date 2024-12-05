using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Shared.Enums;
using TPBlog.Core.Domain.Entity;

namespace TPBlog.Core.Domain.Content
{
    [Table("IC_PostActivityLogs")]
    public class IC_PostActivityLog:EntityBase<Guid>
    {
        //[Key]
        //public Guid Id { get; set; }
        public Guid PostId { get; set; }

        public PostStatus FromStatus { set; get; }

        public PostStatus ToStatus { set; get; }

        //public DateTime DateCreated { get; set; }

        [MaxLength(500)]
        public string? Note { set; get; }

        public Guid UserId { get; set; }
        [MaxLength(250)]
        public string UserName { set; get; }
        public Guid ProjectId { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string ProjectSlug { get; set; }



    }
}