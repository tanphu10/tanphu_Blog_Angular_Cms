using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Core.Repositories
{
    public interface IPostRepository : IRepository<Post, Guid>
    {
        Task<List<Post>> GetPopularPostAsync(int count);
        Task<PageResult<Post>> GetPagingPostAsync(string keyword, Guid? categoryId, int pageIndex = 1, int pageSize = 10);
    }
}
