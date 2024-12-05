using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models;
using TPBlog.Data.SeedWorks;
using static TPBlog.Core.SeedWorks.Contants.Permissions;

namespace TPBlog.Api.Controllers
{
    [Route("api/admin/inventoryCategory")]
    [ApiController]
    public class InventoryCategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public InventoryCategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpPost]
        [Authorize(InventoryCategories.View)]
        public async Task<IActionResult> CreateInventoryCategory([FromBody] CreateUpdateInvtCategoryRequest request)
        {
            request.DateCreated = DateTimeOffset.Now;
            var post = _mapper.Map<CreateUpdateInvtCategoryRequest, IC_InventoryCategory>(request);
            _unitOfWork.IC_InventoryCategories.Add(post);

            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPut]
        //[Authorize(IC_InventoryCategories.Edit)]
        public async Task<IActionResult> UpdateInventoryCategory(Guid id, [FromBody] CreateUpdateInvtCategoryRequest request)
        {
            var post = await _unitOfWork.IC_InventoryCategories.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            post.DateLastModified = DateTimeOffset.Now;

            _mapper.Map(request, post);

            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpDelete]
        //[Authorize(IC_InventoryCategories.Delete)]
        public async Task<IActionResult> DeleteInventoryCategory([FromQuery] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var post = await _unitOfWork.IC_InventoryCategories.GetByIdAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                if (await _unitOfWork.IC_InventoryCategories.HasPost(id))
                {
                    return BadRequest("Danh mục đang chứa bài viết, không thể xóa");
                }
                _unitOfWork.IC_InventoryCategories.Remove(post);
            }
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(InventoryCategories.View)]
        public async Task<ActionResult<InventoryCategoryDto>> GetInventoryCategoryById(Guid id)
        {
            var category = await _unitOfWork.IC_InventoryCategories.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryDto = _mapper.Map<InventoryCategoryDto>(category);
            return Ok(categoryDto);
        }

        [HttpGet]
        [Route("paging")]
        [Authorize(InventoryCategories.View)]
        public async Task<ActionResult<PageResult<PostCategoryDto>>> GetInventoryCategoriesPaging(string? keyword,
            int pageIndex, int pageSize = 10)
        {
            var result = await _unitOfWork.IC_InventoryCategories.GetPagingInventoryCategoryAsync(keyword, pageIndex, pageSize);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(InventoryCategories.View)]
        public async Task<ActionResult<List<InventoryCategoryDto>>> GetInventoryCategories()
        {
            var query = await _unitOfWork.IC_InventoryCategories.GetAllAsync();
            var model = _mapper.Map<List<InventoryCategoryDto>>(query);
            return Ok(model);
        }
    }
}
