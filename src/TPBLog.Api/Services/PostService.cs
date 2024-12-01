using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TPBlog.Api.Extensions;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Domain.Identity;
using TPBlog.Core.Helpers;
using TPBlog.Core.Models.content;
using TPBlog.Data;
using TPBlog.Data.SeedWorks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace TPBlog.Api.Services
{
    public class PostService : IPostService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TPBlogContext _context;
        public PostService(IMapper mapper, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, TPBlogContext context)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task UpdatePostServiceAsync(Guid id, CreateUpdatePostRequest request)
        {
            if (await _unitOfWork.BaiPost.IsSlugAlreadyExisted(request.Slug, id))
            {
                throw new Exception("đã tồn tại slug");
            }
            var post = await _unitOfWork.BaiPost.GetByIdAsync(id);
            if (post == null)
            {
                throw new Exception("không tìm thấy");
            }
            if (post.CategoryId != request.CategoryId)
            {
                var category = await _unitOfWork.PostCategories.GetByIdAsync(request.CategoryId);
                post.CategoryName = category.Name;
                post.CategorySlug = category.Slug;
                post.DateLastModified = DateTimeOffset.Now;
            }
            _mapper.Map(request, post);
            //Process tag 
            if (request.Tags != null && request.Tags.Length > 0)
            {
                foreach (var tagName in request.Tags)
                {
                    var tagSlug = TextHelper.ToUnsignedString(tagName);
                    var tag = await _unitOfWork.Tags.GetBySlug(tagSlug);
                    Guid tagId;
                    if (tag == null)
                    {
                        tagId = Guid.NewGuid();
                        _unitOfWork.Tags.Add(new Tag() { Id = tagId, Name = tagName, Slug = tagSlug });

                    }
                    else
                    {
                        tagId = tag.Id;
                    }
                    // Kiểm tra xem đã có bản ghi PostTags hay chưa
                    var postTagExists = await _unitOfWork.BaiPost.CheckPostTagExists(id, tagId);

                    if (!postTagExists)
                    {
                        await _unitOfWork.BaiPost.AddTagToPost(id, tagId);
                    }
                    //await _unitOfWork.BaiPost.AddTagToPost(id, tagId);
                }
            }
        }

        public async Task CreatePostServiceAsync(CreateUpdatePostRequest request)
        {

            if (await _unitOfWork.BaiPost.IsSlugAlreadyExisted(request.Slug))
            {
                throw new Exception("đã tồn tại slug");
            }
            var post = _mapper.Map<CreateUpdatePostRequest, Post>(request);
            var postId = Guid.NewGuid();
            var category = await _unitOfWork.PostCategories.GetByIdAsync(request.CategoryId);
            post.Id = postId;
            post.CategoryName = category.Name;
            post.CategorySlug = category.Slug;
            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            post.AuthorUserId = userId;
            post.AuthorName = user.GetFullName();
            post.AuthorUserName = user.UserName;
            post.DateCreated = DateTimeOffset.Now;
            var project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId);
            if (project == null)
            {
                throw new Exception("không tồn tại dự án");
            }
            post.ProjectSlug = project.Slug;
            _unitOfWork.BaiPost.Add(post);
            //Process tag 
            if (request.Tags != null && request.Tags.Length > 0)
            {
                foreach (var tagName in request.Tags)
                {
                    var tagSlug = TextHelper.ToUnsignedString(tagName);
                    var tag = await _unitOfWork.Tags.GetBySlug(tagSlug);
                    Guid tagId;
                    if (tag == null)
                    {
                        tagId = Guid.NewGuid();
                        _unitOfWork.Tags.Add(new Tag() { Id = tagId, Name = tagName, Slug = tagSlug, ProjectSlug = project.Slug });

                    }
                    else
                    {
                        tagId = tag.Id;
                    }
                    await _unitOfWork.BaiPost.AddTagToPost(postId, tagId);
                }
            }
        }

        public async Task<List<PostInListDto>> GetAllPostAsync()
        {
            var entity = _context.Posts.AsQueryable();
            return await _mapper.ProjectTo<PostInListDto>(entity).ToListAsync();

        }
    }
}
