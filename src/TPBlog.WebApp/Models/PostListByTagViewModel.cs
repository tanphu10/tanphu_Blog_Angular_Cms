using TPBlog.Core.Models;
using TPBlog.Core.Models.content;

namespace TPBlog.WebApp.Models
{
    public class PostListByTagViewModel
    {
        public TagDto Tag { get; set; }
        public PageResult<PostInListDto> Posts { get; set; }

    }
}
