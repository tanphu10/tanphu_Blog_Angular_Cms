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
        ITaskRepository IC_Tasks { get; }
        IPostRepository IC_Posts { get; }
        IPostCategoryRepository IC_PostCategories { get; }
        ISeriesRepository IC_Series { get; }
        IProjectRepository IC_Projects { get; }
        ITransactionRepository IC_Transactions { get; }
        ITagRepository IC_Tags { get; }
        IUserRepository Users { get; }
        IInventoryRepository IC_Inventories { get; }
        IInventoryCategoryRepository IC_InventoryCategories { get; }

        IProductRepository IC_Products { get; }
        IProCategoryRepository IC_ProCategories { get; }
        IAnnouncementRepository IC_Announcements { get; }
        IAnnouncementUserRepository IC_AnnouncementUsers { get; }
        Task<int> CompleteAsync();
    }
}
