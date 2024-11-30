using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Core.Repositories
{
    public interface IInventoryCategoryRepository : IRepository<InventoryCategory, Guid>
    {
        Task<PageResult<InventoryCategoryDto>> GetPagingInventoryCategoryAsync(string? keyword, int pageIndex = 1, int pageSize = 10);
        Task<bool> HasPost(Guid invtCategoryId);
        Task<InventoryCategoryDto> GetBySlug(string Slug);

    }
}
