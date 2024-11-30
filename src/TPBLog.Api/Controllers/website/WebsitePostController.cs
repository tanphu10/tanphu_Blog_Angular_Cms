using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TPBlog.Api.Extensions;
using TPBlog.Api.Services;
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

        public WebsitePostController(IMapper mapper, IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IPostService postService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _postService = postService;
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
        public async Task<ActionResult<PageResult<PostInListDto>>> GetPostWebsitePaging(string? keyword, string? categorySlug,
                  int pageIndex = 1, int pageSize = 10)
        {
            var userId = User.GetUserId();
            var category = await _unitOfWork.PostCategories.GetBySlug(categorySlug);
            //if (category == null)
            //{
            //    return NotFound("không tìm thấy category");
            //}
            var result = await _unitOfWork.BaiPost.GetAllPaging(keyword, userId, category.Id, pageIndex, pageSize);
            return Ok(result);
        }
    }
}
