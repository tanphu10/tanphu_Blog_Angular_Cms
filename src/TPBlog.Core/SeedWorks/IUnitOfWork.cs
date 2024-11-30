using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Repositories;

namespace TPBlog.Data.SeedWorks
{
    public interface IUnitOfWork
    {
        IPostRepository BaiPost { get; }
        IPostCategoryRepository PostCategories { get; }
        ISeriesRepository Series { get; }
        IProjectRepository Projects { get; }
        ITransactionRepository Transactions { get; }
        ITagRepository Tags { get; }
        IUserRepository Users { get; }
        IInventoryRepository Inventories { get; }
        IInventoryCategoryRepository InventoryCategories { get; }

        IProductRepository Products { get; }
        IProCategoryRepository ProCategories { get; }
        IAnnouncementRepository Announcements { get; }
        IAnnouncementUserRepository AnnouncementUsers { get; }
        Task<int> CompleteAsync();
    }
}
