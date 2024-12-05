using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPBlog.Core.Domain.Content
{
    [Table("IC_PostInSeries")]
    [PrimaryKey(nameof(PostId), nameof(SeriesId))]
    public class IC_PostInSeries
    {
        public Guid PostId { get; set; }
        public Guid SeriesId { get; set; }
        public int DisplayOrder { get; set; }
    }
}