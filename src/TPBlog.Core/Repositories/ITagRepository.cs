using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Core.Repositories
{
    public interface ITagRepository : IRepository<IC_Tag, Guid>
    {
        Task<List<string>> GetAllTags();
        Task<TagDto?> GetBySlug(string slug);
    }
}
