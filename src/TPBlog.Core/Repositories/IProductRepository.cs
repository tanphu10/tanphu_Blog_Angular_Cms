using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models;
using TPBlog.Data.SeedWorks;
using Microsoft.Extensions.DependencyInjection;

namespace TPBlog.Core.Repositories
{
    public interface IProductRepository:IRepository<Product, Guid>
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(Guid id);
        Task<IEnumerable<Product>> GetProductByNoAsync(string productNo);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid id);
        Task<PageResult<ProductInListDto>> GetAllProductPaging(string? keyword, int pageIndex = 1, int pageSize = 10);

    }

}
