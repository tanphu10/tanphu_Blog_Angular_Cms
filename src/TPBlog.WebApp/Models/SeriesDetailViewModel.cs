using TPBlog.Core.Models;
using TPBlog.Core.Models.content;

namespace TPBlog.WebApp.Models
{
    public class SeriesDetailViewModel
    {
        public PageResult<PostInListDto> Posts { get; set; }
        public SeriesDto Series { get; set; }

    }
}
