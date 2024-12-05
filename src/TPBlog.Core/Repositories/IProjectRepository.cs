using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models;
using TPBlog.Data.SeedWorks;
using TPBlog.Core.Domain.Content;

namespace TPBlog.Core.Repositories
{
    public interface IProjectRepository : IRepository<IC_Project, Guid>
    {
        Task<PageResult<ProjectInListDto>> GetAllPaging(string? keyword, int pageIndex = 1, int pageSize = 10);
        Task AddPostToProject(Guid projectId, Guid postId, int sortOrder);
        Task RemovePostToProject(Guid projectId, Guid postId);
        Task<List<PostInListDto>> GetAllPostsInProject(Guid projectId);
        Task<PageResult<PostInListDto>> GetPostsInProjectPaging(string? slug, int pageIndex = 1, int pageSize = 10);
        Task<ProjectDto> GetBySlug(string slug);
        Task<bool> IsPostInProject(Guid projectId, Guid postId);
        Task<bool> HasPost(Guid projectId);
    }
}
