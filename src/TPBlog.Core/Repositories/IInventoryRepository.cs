using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Core.Repositories
{
    public interface IInventoryRepository :IRepository<InventoryEntry, Guid>

    {
        Task<int> GetStockQuantity(string itemNo);

        Task<List<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo);
        Task<PageResult<InventoryInListDto>> GetAllByItemNoPagingAsync(string? keyword,Guid? projectId,string? categorySlug,int pageIndex = 1, int pageSize = 10);
        Task<PageResult<InventoryInListDto>> GetAllByCategoryPagingAsync(string? keyword,Guid? categoryId,Guid? projectId,int pageIndex = 1, int pageSize = 10);

        Task<InventoryEntryDto> GetByIdAsync(Guid id);
        Task<InventoryEntryDto> GetBySlug(string slug);
        Task<bool> IsSlugAlreadyExisted(string slug, Guid? currentId = null);

        Task<InventoryEntryDto> PurchaseItemAsync(string itemNo, PurchaseProductDto model);
        Task<InventoryEntryDto> SalesItemAsync(string itemNo, SalesProductDto model);

        Task DeleteByDocumentNoAsync(string documentNo);
        Task DeleteByIdAsync(Guid id);
        Task<string> SalesOrderAsync(SalesOrderDto model);
    }
}
