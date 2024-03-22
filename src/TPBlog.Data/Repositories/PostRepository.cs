﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Domain.Identity;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models.system;
using TPBlog.Core.Repositories;
using TPBlog.Core.SeedWorks.Contants;
using TPBlog.Data;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Data.Repositories
{
    public class PostRepository : RepositoryBase<Post, Guid>, IPostRepository
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public PostRepository(TPBlogContext context, IMapper mapper,
            UserManager<AppUser> userManager) : base(context)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<PageResult<PostInListDto>> GetAllPaging(string? keyword, Guid currentUserId, Guid? categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var user = await _userManager.FindByIdAsync(currentUserId.ToString());
            if (user == null)
            {
                throw new Exception("Không tồn tại user");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var canApprove = false;
            if (roles.Contains(Roles.Admin))
            {
                canApprove = true;
            }
            else
            {
                canApprove = await _context.RoleClaims.AnyAsync(x => roles.Contains(x.RoleId.ToString())
                           && x.ClaimValue == Permissions.Posts.Approve);
            }

            var query = _context.Posts.AsQueryable();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            if (!canApprove)
            {
                query = query.Where(x => x.AuthorUserId == currentUserId);
            }

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
        public async Task<List<SeriesInListDto>> GetAllSeries(Guid id)
        {
            var query = from pis in _context.PostInSeries
                        join s in _context.Series
                        on pis.SeriesId equals s.Id
                        where pis.PostId == id
                        select s;
            return await _mapper.ProjectTo<SeriesInListDto>(query).ToListAsync();
        }
        public IEnumerable<Post> GetPopularPosts(int count)
        {
            return _context.Posts.OrderByDescending(d => d.ViewCount).Take(count).ToList();
        }

        public Task<bool> IsSlugAlreadyExisted(string slug, Guid? currentId = null)
        {
            if (currentId.HasValue)
            {
                return _context.Posts.AnyAsync(x => x.Slug == slug && x.Id != currentId.Value);
            }
            return _context.Posts.AnyAsync(x => x.Slug == slug);
        }
        public async Task Approve(Guid id, Guid currentUserId)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                throw new Exception("Không tồn tại bài viết");
            }
            var user = await _context.Users.FindAsync(currentUserId);
            await _context.PostActivityLogs.AddAsync(new PostActivityLog
            {
                Id = Guid.NewGuid(),
                FromStatus = post.Status,
                ToStatus = PostStatus.Published,
                UserId = currentUserId,
                UserName = user.UserName,
                PostId = id,
                Note = $"{user?.UserName} duyệt bài"
            });
            post.Status = PostStatus.Published;
            _context.Posts.Update(post);
        }
        public async Task ReturnBack(Guid id, Guid currentUserId, string note)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                throw new Exception("Không tồn tại bài viết");
            }
            var user = await _userManager.FindByIdAsync(currentUserId.ToString());
            await _context.PostActivityLogs.AddAsync(new PostActivityLog
            {
                FromStatus = post.Status,
                ToStatus = PostStatus.Rejected,
                UserId = currentUserId,
                UserName = user.UserName,
                PostId = post.Id,
                Note = note
            });
            post.Status = PostStatus.Rejected;
            _context.Posts.Update(post);
        }
        public async Task<string> GetReturnReason(Guid id)
        {
            var activity = await _context.PostActivityLogs
                .Where(x => x.PostId == id && x.ToStatus == PostStatus.Rejected)
                .OrderByDescending(x => x.DateCreated)
                .FirstOrDefaultAsync();
            return activity?.Note;
        }
        public async Task<bool> HasPublishInLast(Guid id)
        {
            var hasPublished =
                await _context.PostActivityLogs.CountAsync(x => x.PostId == id
                && x.ToStatus == PostStatus.Published);
            return hasPublished > 0;
        }
        public async Task<List<PostActivityLogDto>> GetActivityLogs(Guid id)
        {
            var query = _context.PostActivityLogs.Where(x => x.PostId == id)
                .OrderByDescending(x => x.DateCreated);
            return await _mapper.ProjectTo<PostActivityLogDto>(query).ToListAsync();
        }
        public async Task SendToApprove(Guid id, Guid currentUserId)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                throw new Exception("Không tồn tại bài viết");
            }
           //checked lại ở đây vì sao không dùng -context.Findbyit 
            var user = await _userManager.FindByIdAsync(currentUserId.ToString());
            if (user == null)
            {
                throw new Exception("Không tồn tại user");
            }
            await _context.PostActivityLogs.AddAsync(new PostActivityLog
            {
                FromStatus = post.Status,
                ToStatus = PostStatus.WaitingForApproval,
                UserId = currentUserId,
                PostId = post.Id,
                UserName = user.UserName,
                Note = $"{user.UserName} gửi bài chờ duyệt"
            });
            post.Status = PostStatus.WaitingForApproval;
            _context.Posts.Update(post);
        }
    }
}