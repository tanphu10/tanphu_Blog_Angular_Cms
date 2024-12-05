using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Core.Repositories
{
    public interface IPostCategoryRepository : IRepository<IC_PostCategory, Guid>
    {
        Task<PageResult<PostCategoryDto>> GetPagingPostCategoryAsync(string? keyword,Guid? projectId,  int pageIndex = 1, int pageSize = 10);
        Task<bool> HasPost(Guid categoryId);
        Task<PostCategoryDto> GetBySlug(string Slug);

    }
        
        
       
}
