using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;
using TPBlog.Data.SeedWorks;
using static TPBlog.Core.SeedWorks.Contants.Permissions;

namespace TPBlog.Api.Controllers
{
    [Route("api/admin/IC_Products")]
    [ApiController]
    public class ProductController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region CRUD

        [HttpGet]
        [Route("paging")]
        [Authorize(Products.View)]
        public async Task<ActionResult<PageResult<ProductInListDto>>> GetProductPaging(string? keyword, Guid? categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var result = await _unitOfWork.IC_Products.GetAllProductPaging(keyword, categoryId, pageIndex, pageSize);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            //count++;
            //if (count < 3)
            //{
            //    Thread.Sleep(5000);
            //}
            var IC_Products = await _unitOfWork.IC_Products.GetProductsAsync();
            var result = _mapper.Map<IEnumerable<ProductDto>>(IC_Products);
            return Ok(result);
        }
        [HttpGet("{id:Guid}")]
        [Authorize(Products.View)]

        public async Task<ActionResult<ProductDto>> GetProductById([Required] Guid id)
        {
            var product = await _unitOfWork.IC_Products.GetProductAsync(id);
            if (product == null)
                return NotFound();
            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Products.Create)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            var product = _mapper.Map<IC_Product>(productDto);
            await _unitOfWork.IC_Products.CreateProductAsync(product);
            await _unitOfWork.CompleteAsync();
            //var result = _mapper.Map<ProductDto>(product);
            return Ok();
        }

        [HttpPut("{id:Guid}")]
        [Authorize(Products.Edit)]

        //[Authorize]
        //[ClaimRequirement(FunctionCode.PRODUCT, CommandCode.UPDATE)]
        public async Task<IActionResult> UpdateProduct([Required] Guid id, [FromBody] UpdateProductDto productDto)
        {
            var product = await _unitOfWork.IC_Products.GetProductAsync(id);
            if (product is null) return NotFound();

            var updateProduct = _mapper.Map(productDto, product);
            await _unitOfWork.IC_Products.UpdateProductAsync(updateProduct);
            //var result = _mapper.Map<ProductDto>(product);
            return Ok();
        }
        [HttpDelete]
        [Authorize(Products.Delete)]
        public async Task<IActionResult> DeleteProduct([Required] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var product = await _unitOfWork.IC_Products.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                _unitOfWork.IC_Products.Remove(product);
            }
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();

        }
        #endregion

        #region Additional Resources

        [HttpGet("get-product-by-no/{productNo}")]
        [Authorize(Products.View)]

        public async Task<IActionResult> GetProductByNo([Required] string productNo)
        {
            var product = await _unitOfWork.IC_Products.GetProductByNoAsync(productNo);
            if (product == null) return NotFound();

            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        #endregion


    }
}
