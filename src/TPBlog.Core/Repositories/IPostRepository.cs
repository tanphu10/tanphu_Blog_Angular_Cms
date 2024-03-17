using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Core.Repositories
{
    public interface IPostRepository : IRepository<Post, Guid>
    {
        Task<List<Post>> GetPopularPostAsync(int count);
        Task<PageResult<PostInListDto>> GetPagingPostAsync(string? keyword, Guid? categoryId, int pageIndex = 1, int pageSize = 10);
    }
}
