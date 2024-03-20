using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Repositories;
using TPBlog.Data.Repositories;

namespace TPBlog.Data.SeedWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TPBlogContext _context;
        public UnitOfWork(TPBlogContext context, IMapper mapper)
        {
            _context = context;
            BaiPost = new PostRepository(context, mapper);
            PostCategories = new PostCategoryRepository(context, mapper);
            Tags = new TagRepositiory(context, mapper);
        }
        public IPostRepository BaiPost { get; private set; }
        public IPostCategoryRepository PostCategories { get; private set; }

        public ITagRepository Tags { get; private set; }
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
