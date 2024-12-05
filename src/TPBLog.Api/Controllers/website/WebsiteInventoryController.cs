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
    [Route("api/website/inventory")]
    [ApiController]
    public class WebsiteInventoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPostService _postService;
        private readonly UserManager<AppUser> _userManager;

        public WebsiteInventoryController(IMapper mapper, IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IPostService postService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _postService = postService;
        }
        [HttpGet]
        [Route("detail/{slug}")]
        [Authorize(Inventories.View)]
        public async Task<ActionResult<InventoryEntryDto>> GetInventoryWebsiteBySlug(string slug)
        {
            var post = await _unitOfWork.IC_Inventories.GetBySlug(slug);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        [HttpGet]
        [Route("GetAll")]
        [Authorize(Inventories.View)]
        public async Task<ActionResult<InventoryInListDto>> GetAllWebsiteInventory()
        {
            var data = await _unitOfWork.IC_Inventories.GetAllAsync();
            return Ok(data);
        }
        [HttpGet]
        [Route("paging")]
        [Authorize(Inventories.View)]
        public async Task<ActionResult<PageResult<InventoryInListDto>>> GetIntentoryWebsitePaging(string? keyword, string? categorySlug,Guid projectId,
                  int pageIndex = 1, int pageSize = 10)
        {
            //var userId = User.GetUserId();
            var category = await _unitOfWork.IC_InventoryCategories.GetBySlug(categorySlug);
            //if (category == null)
            //{
            //    return NotFound("không tìm thấy category");
            //}
            var result = await _unitOfWork.IC_Inventories.GetAllByCategoryPagingAsync(keyword,  category.Id,projectId, pageIndex, pageSize);
            return Ok(result);
        }
    }
}
