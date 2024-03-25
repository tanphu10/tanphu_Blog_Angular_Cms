using TPBlog.Core.Domain.Content;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Core.Repositories
{
    public interface ITagRepository : IRepository<Tag, Guid>
    {
    }
}
