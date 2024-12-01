using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TPBlog.Api.Extensions;
using TPBlog.Api.Services;
using TPBlog.Api.Services.IServices;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Domain.Identity;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;
using TPBlog.Data.SeedWorks;
using static TPBlog.Core.SeedWorks.Contants.Permissions;

namespace TPBlog.Api.Controllers.website
{
    [Route("api/website/post")]
    [ApiController]
    public class WebsitePostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPostService _postService;
        private readonly UserManager<AppUser> _userManager;

        private readonly IPermissionService _permissionService;
        public WebsitePostController(IMapper mapper, IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IPostService postService, IPermissionService permissionService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _postService = postService;
            _permissionService = permissionService;
        }
        [HttpGet]
        [Route("detail/{slug}")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<PostDto>> GetPostWebsiteBySlug(string slug)
        {
            var post = await _unitOfWork.BaiPost.GetBySlug(slug);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        [HttpGet]
        [Route("GetAll")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<PostInListDto>> GetAllWebsitePost()
        {
            var data = await _postService.GetAllPostAsync();
            return Ok(data);
        }
        [HttpGet]
        [Route("paging")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<PageResult<PostInListDto>>> GetPostWebsitePaging(string? keyword, string? categorySlug, string? projectSlug,
                  int pageIndex = 1, int pageSize = 10)
        {
            var userId = User.GetUserId();
            var category = await _unitOfWork.PostCategories.GetBySlug(categorySlug);
            var project = await _unitOfWork.Projects.GetBySlug(projectSlug);
            var userPermissions = await _permissionService.UserHasPermissionForProjectAsync();

            var query = await _unitOfWork.BaiPost.GetAllPaging(keyword, userId, category.Id, project.Id, pageIndex, pageSize);
            var allowedProjects = query.Results.Where(p => userPermissions.Contains($"Permissions.Projects.{p.ProjectSlug}"));
            //var result = await _unitOfWork.BaiPost.GetAllPaging(keyword, userId, category.Id, project.Id, pageIndex, pageSize);
            query.Results = allowedProjects.ToList();

            return Ok(query);
        }
    }
}
