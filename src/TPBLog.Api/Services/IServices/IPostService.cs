using TPBlog.Core.Models.content;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Api.Services
{
    public interface IPostService: IScopedService
    {
        Task CreatePostServiceAsync(CreateUpdatePostRequest request);
        Task UpdatePostServiceAsync(Guid id,CreateUpdatePostRequest request);

        Task<List<PostInListDto>> GetAllPostAsync();
    }
}
