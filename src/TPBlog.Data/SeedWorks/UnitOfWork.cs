﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Identity;
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
            BaiPost = new PostRepository(context, mapper, userManager);
            PostCategories = new PostCategoryRepository(context, mapper);
            Tags = new TagRepositiory(context, mapper);
            Transactions = new TransactionRepository(context, mapper);
            Series = new SeriesRepository(context, mapper);
            Users = new UserRepository(context, mapper);
            Projects = new ProjectRepository(context, mapper);
            Inventories = new InventoryRepository(context, mapper);
            Products = new ProductRepository(context, mapper);
            ProCategories = new ProCategoryRepository(context, mapper);
            Announcements = new AnnouncementRepository(context, mapper);
            AnnouncementUsers = new AnnouncementUserRepository(context);
            InventoryCategories = new InventoryCategoryRepository(context,mapper);
        }
        public IPostRepository BaiPost { get; private set; }
        public IPostCategoryRepository PostCategories { get; private set; }
        public ITransactionRepository Transactions { get; private set; }
        public ISeriesRepository Series { get; private set; }
        public ITagRepository Tags { get; private set; }
        public IUserRepository Users { get; private set; }

        public IProjectRepository Projects { get; private set; }

        public IInventoryRepository Inventories { get; private set; }
        public IProductRepository Products { get; private set; }

        public IProCategoryRepository ProCategories { get; private set; }
        public IAnnouncementRepository Announcements { get; private set; }
        public IAnnouncementUserRepository AnnouncementUsers { get; private set; }

        public IInventoryCategoryRepository InventoryCategories { get; private set; }
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
