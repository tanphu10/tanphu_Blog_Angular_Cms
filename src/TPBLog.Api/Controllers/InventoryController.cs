using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection.Metadata;
using TPBlog.Api.Services;
using TPBlog.Api.Services.IServices;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Domain.Identity;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;
using TPBlog.Core.Repositories;
using TPBlog.Data.SeedWorks;
using static TPBlog.Core.SeedWorks.Contants.Permissions;

namespace TPBlog.Api.Controllers
{
    [Route("api/admin/inventory")]
    [ApiController]
    public class InventoryController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public InventoryController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        [Route("paging")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<PageResult<InventoryEntryDto>>> GetInventoryPaging(string? keyword,
         int pageIndex, int pageSize = 10)
        {
            var result = await _unitOfWork.Inventories.GetAllByItemNoPagingAsync(keyword, pageIndex, pageSize);
            return Ok(result);
        }
        [Route("items/{itemNo}", Name = "GetAllByItemNo")]
        [ProducesResponseType(typeof(IEnumerable<InventoryEntryDto>), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<ActionResult<List<InventoryEntryDto>>> GetAllByItemNo([Required] string itemNo)
        {
            var result = await _unitOfWork.Inventories.GetAllByItemNoAsync(itemNo);
            return Ok(result);
        }

        [Route("stock-quantity/{itemNo}", Name = "GetStockQuantity")]
        [ProducesResponseType(typeof(IEnumerable<InventoryEntryDto>), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<ActionResult<List<InventoryEntryDto>>> GetStock([Required] string itemNo)
        {
            var result = await _unitOfWork.Inventories.GetStockQuantity(itemNo);
            return Ok(result);
        }


        [Route("{id}", Name = "GetAllById")]
        [ProducesResponseType(typeof(IEnumerable<InventoryEntryDto>), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<ActionResult<InventoryEntryDto>> GetInventoryById([Required] Guid id)
        {
            var result = await _unitOfWork.Inventories.GetByIdAsync(id);
            return Ok(result);
        }

        [Route("purchase/{itemNo}", Name = "PurchaseOrder")]
        [ProducesResponseType(typeof(IEnumerable<InventoryEntryDto>), (int)HttpStatusCode.OK)]
        [HttpPost]
        public async Task<ActionResult<InventoryEntryDto>> PurchaseOrder([Required] string itemNo, [FromBody] PurchaseProductDto model)
        {
            var result = await _unitOfWork.Inventories.PurchaseItemAsync(itemNo, model);
            return Ok(result);
        }
        [Route("sales/{itemNo}", Name = "SaleItem")]
        [ProducesResponseType(typeof(InventoryEntryDto), (int)HttpStatusCode.OK)]
        [HttpPost]
        public async Task<ActionResult<InventoryEntryDto>> SaleItem([Required] string itemNo, [FromBody] SalesProductDto model)
        {
            var result = await _unitOfWork.Inventories.SalesItemAsync(itemNo, model);
            return Ok(result);
        }
        [Route("sales/order-no/{orderNo}", Name = "SaleOrder")]
        [ProducesResponseType(typeof(CreatedSalesOrderSuccessDto), (int)HttpStatusCode.OK)]
        [HttpPost]
        public async Task<ActionResult<InventoryEntryDto>> SaleOrder([Required] string orderNo, [FromBody] SalesOrderDto model)
        {
            model.OrderNo = orderNo;
            var documentNo = await _unitOfWork.Inventories.SalesOrderAsync(model);
            var result = new CreatedSalesOrderSuccessDto(documentNo);
            return Ok(result);
        }
        [HttpDelete("{id}", Name = "DeleteById")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteById([Required] Guid id)
        {
            await _unitOfWork.Inventories.DeleteByIdAsync(id);
            return NoContent();
        }
        [Route("document-no/{documentNo}", Name = "DeleteByDocumentNo")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteByDocumentNo([Required] string documentNo)
        {
            await _unitOfWork.Inventories.DeleteByDocumentNoAsync(documentNo);
            return NoContent();
        }


    }
}
