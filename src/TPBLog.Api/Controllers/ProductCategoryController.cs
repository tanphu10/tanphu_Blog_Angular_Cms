using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Api.Controllers
{
    [Route("api/admin/product-category")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductCategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpPost]
        //[Authorize(Posts.View)]

        public async Task<IActionResult> CreateProductCategory([FromBody] CreateUpdateProductCategoryRequest request)
        {
            request.DateCreated = DateTimeOffset.Now;
            var post = _mapper.Map<CreateUpdateProductCategoryRequest, ProductCategory>(request);
            _unitOfWork.ProCategories.Add(post);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPut]
        //[Authorize(PostCategories.Edit)]
        public async Task<IActionResult> UpdateProductCategory(Guid id, [FromBody] CreateUpdateProductCategoryRequest request)
        {
            var post = await _unitOfWork.ProCategories.GetByIdAsync(id);
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
        //[Authorize(PostCategories.Delete)]
        public async Task<IActionResult> DeleteProductCategory([FromQuery] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var post = await _unitOfWork.ProCategories.GetByIdAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                if (await _unitOfWork.ProCategories.HasProduct(id))
                {
                    return BadRequest("Danh mục đang chứa sản phẩm, không thể xóa");
                }
                _unitOfWork.ProCategories.Remove(post);
            }
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpGet]
        [Route("{id}")]
        //[Authorize(PostCategories.View)]
        public async Task<ActionResult<ProductCategoryDto>> GetProductCategoryById(Guid id)
        {
            var category = await _unitOfWork.ProCategories.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryDto = _mapper.Map<ProductCategoryDto>(category);
            return Ok(categoryDto);
        }

        [HttpGet]
        [Route("paging")]
        //[Authorize(PostCategories.View)]
        public async Task<ActionResult<PageResult<ProductCategoryDto>>> GetProductCategoriesPaging(string? keyword,
            int pageIndex=1, int pageSize = 10)
        {
            var result = await _unitOfWork.ProCategories.GetPagingProductCategoryAsync(keyword, pageIndex, pageSize);
            return Ok(result);
        }

        [HttpGet]
        //[Authorize(PostCategories.View)]
        public async Task<ActionResult<List<ProductCategoryDto>>> GetProductCategories()
        {
            var query = await _unitOfWork.ProCategories.GetAllAsync();
            var model = _mapper.Map<List<ProductCategoryDto>>(query);
            return Ok(model);
        }
    }
}
