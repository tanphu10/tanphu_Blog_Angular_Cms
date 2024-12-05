using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Domain.Identity;
using TPBlog.Core.Domain.Royalty;
using TPBlog.Core.Repositories;
using TPBlog.Data.Repositories;

namespace TPBlog.Data.SeedWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TPBlogContext _context;

        public UnitOfWork(TPBlogContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            IC_Posts = new PostRepository(context, mapper, userManager);
            IC_PostCategories = new PostCategoryRepository(context, mapper);
            IC_Tags = new TagRepositiory(context, mapper);
            IC_Transactions = new TransactionRepository(context, mapper);
            IC_Series = new SeriesRepository(context, mapper);
            Users = new UserRepository(context, mapper);
            IC_Projects = new ProjectRepository(context, mapper);
            IC_Inventories = new InventoryRepository(context, mapper);
            IC_Products = new ProductRepository(context, mapper);
            IC_ProCategories = new ProCategoryRepository(context, mapper);
            IC_Announcements = new AnnouncementRepository(context, mapper);
            IC_AnnouncementUsers = new AnnouncementUserRepository(context);
            IC_InventoryCategories = new InventoryCategoryRepository(context,mapper);
            IC_Tasks = new TaskRepository(context,mapper);
        }
        public ITaskRepository IC_Tasks { get; private set; }

        public IPostRepository IC_Posts { get; private set; }
        public IPostCategoryRepository IC_PostCategories { get; private set; }
        public ITransactionRepository IC_Transactions { get; private set; }
        public ISeriesRepository IC_Series { get; private set; }
        public ITagRepository IC_Tags { get; private set; }
        public IUserRepository Users { get; private set; }

        public IProjectRepository IC_Projects { get; private set; }

        public IInventoryRepository IC_Inventories { get; private set; }
        public IProductRepository IC_Products { get; private set; }

        public IProCategoryRepository IC_ProCategories { get; private set; }
        public IAnnouncementRepository IC_Announcements { get; private set; }
        public IAnnouncementUserRepository IC_AnnouncementUsers { get; private set; }

        public IInventoryCategoryRepository IC_InventoryCategories { get; private set; }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
