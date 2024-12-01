using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Core.Repositories
{
    public interface IPostRepository : IRepository<Post, Guid>
    {
        Task<PageResult<PostInListDto>> GetAllPaging(string? keyword, Guid currentUserId, Guid? categoryId, Guid? projectId, int pageIndex = 1, int pageSize = 10);
        Task<bool> IsSlugAlreadyExisted(string slug, Guid? currentId = null);
        Task<bool> CheckPostTagExists(Guid id, Guid tagId);
        Task<List<SeriesInListDto>> GetAllSeries(Guid postId);
        Task Approve(Guid id, Guid currentUserId);
        Task SendToApprove(Guid id, Guid currentUserId);
        Task ReturnBack(Guid id, Guid currentUserId, string note);
        Task<string> GetReturnReason(Guid id);
        Task<bool> HasPublishInLast(Guid id);
        Task<List<PostActivityLogDto>> GetActivityLogs(Guid id);
        Task<List<Post>> GetListUnpaidPublishPosts(Guid userId);
        Task<List<PostInListDto>> GetLatestPublishPost(int top);
        Task<PageResult<PostInListDto>> GetPostByCategoryPaging(string categorySlug, int pageIndex = 1, int pageSize = 10);
        Task<PostDto> GetBySlug(string slug);
        Task AddTagToPost(Guid postId,Guid tagId);
        Task<List<string>> GetTagsByPostId(Guid postId);
        Task<List<TagDto>> GetTagsObjectsByPostId(Guid postId);

        Task<PageResult<PostInListDto>> GetPostByTagPaging(string tagSlug, int pageIndex = 1, int pageSize = 10);
        Task<PageResult<PostInListDto>> GetPostByUserPaging(string keyword,Guid userId, int pageIndex = 1, int pageSize = 10);

    }
}
