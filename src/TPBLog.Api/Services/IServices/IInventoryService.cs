using TPBlog.Core.Models.content;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Api.Services.IServices
{
    public interface IInventoryService: IScopedService
    {
        Task<List<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo);
        //Task<PagedList<InventoryEntryDto>> GetAllByItemNoPagingAsync(GetInventoryPagingQuery query);
        Task<InventoryEntryDto> GetByIdAsync(Guid id);
        Task<InventoryEntryDto> PurchaseItemAsync(string itemNo, PurchaseProductDto model);
        Task<InventoryEntryDto> SalesItemAsync(string itemNo, SalesProductDto model);

        Task DeleteByDocumentNoAsync(string documentNo);
        Task<string> SalesOrderAsync(SalesOrderDto model);
    }
}
