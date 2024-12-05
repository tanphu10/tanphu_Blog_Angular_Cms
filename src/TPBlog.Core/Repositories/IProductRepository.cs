using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models;
using TPBlog.Data.SeedWorks;
using Microsoft.Extensions.DependencyInjection;

namespace TPBlog.Core.Repositories
{
    public interface IProductRepository : IRepository<IC_Product, Guid>
    {
        Task<IEnumerable<IC_Product>> GetProductsAsync();
        Task<IC_Product> GetProductAsync(Guid id);
        Task<IEnumerable<IC_Product>> GetProductByNoAsync(string productNo);
        Task CreateProductAsync(IC_Product product);
        Task UpdateProductAsync(IC_Product product);
        Task DeleteProductAsync(Guid id);
        Task<PageResult<ProductInListDto>> GetAllProductPaging(string? keyword,Guid? categoryId, int pageIndex = 1, int pageSize = 10);

    }

}
