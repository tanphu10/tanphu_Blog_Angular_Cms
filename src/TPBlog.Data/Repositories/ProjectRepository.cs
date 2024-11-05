using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models;
using TPBlog.Core.Repositories;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Data.Repositories
{
    public class ProjectRepository : RepositoryBase<Project, Guid>, IProjectRepository
    {
        private readonly IMapper _mapper;
        public ProjectRepository(TPBlogContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task AddPostToProject(Guid projectId, Guid postId, int sortOrder)
        {
            var postInProject = await _context.PostInProject.FirstOrDefaultAsync(x => x.PostId == postId && x.ProjectId == projectId);
            if (postInProject == null)
            {
                await _context.PostInProject.AddAsync(new PostInProject()
                {
                    ProjectId = projectId,
                    PostId = postId,
                    DisplayOrder = sortOrder
                });
            }
        }

        public async Task<PageResult<ProjectInListDto>> GetAllPaging(string? keyword, int pageIndex = 1, int pageSize = 10)
        {
            var query = _context.Series.AsQueryable();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }

            var totalRow = await query.CountAsync();

            query = query.OrderByDescending(x => x.DateCreated)
               .Skip((pageIndex - 1) * pageSize)
               .Take(pageSize);

            return new PageResult<ProjectInListDto>
            {
                Results = await _mapper.ProjectTo<ProjectInListDto>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };
        }

        public async Task<List<PostInListDto>> GetAllPostsInProject(Guid projectId)
        {
            var query = from pip in _context.PostInProject
                        join p in _context.Posts
                        on pip.PostId equals p.Id
                        where pip.ProjectId == projectId
                        select p;
            return await _mapper.ProjectTo<PostInListDto>(query).ToListAsync();
        }

        public async Task<bool> IsPostInProject(Guid projectId, Guid postId)
        {
            return await _context.PostInProject.AnyAsync(x => x.ProjectId == projectId && x.PostId == postId);
        }

        public async Task RemovePostToProject(Guid projectId, Guid postId)
        {
            var postInProject = await _context.PostInProject
                .FirstOrDefaultAsync(x => x.PostId == postId && x.ProjectId == projectId);
            if (postInProject != null)
            {
                _context.PostInProject.Remove(postInProject);
            }
        }
        public async Task<bool> HasPost(Guid projectId)
        {
            return await _context.PostInProject.AnyAsync(x => x.ProjectId == projectId);
        }

        public async Task<PageResult<PostInListDto>> GetPostsInProjectPaging(string? slug, int pageIndex = 1, int pageSize = 10)
        {

            var query = from pis in _context.PostInProject
                        join s in _context.Project on pis.ProjectId equals s.Id
                        join p in _context.Posts on pis.PostId equals p.Id
                        where s.Slug == slug
                        select p;

            var totalRow = await query.CountAsync();
            query = query.OrderByDescending(x => x.DateCreated)
               .Skip((pageIndex - 1) * pageSize)
               .Take(pageSize);
            return new PageResult<PostInListDto>
            {
                Results = await _mapper.ProjectTo<PostInListDto>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };
        }
        public async Task<ProjectDto> GetBySlug(string slug)
        {
            var series = await _context.Project.FirstOrDefaultAsync(x => x.Slug == slug);
            return _mapper.Map<ProjectDto>(series);
        }


    }
}
