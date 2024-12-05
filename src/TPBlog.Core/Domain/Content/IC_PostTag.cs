using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPBlog.Core.Domain.Content
{
    [Table("IC_PostTags")]
    [PrimaryKey(nameof(PostId), nameof(TagId))]
    public class IC_PostTag
    {
        public Guid PostId { set; get; }
        public Guid TagId { set; get; }
    }
}