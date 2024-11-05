using AutoMapper;
using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;
using TPBlog.Core.Repositories;
using TPBlog.Data.SeedWorks;
using static TPBlog.Core.SeedWorks.Contants.Permissions;

namespace TPBlog.Data.Repositories
{
    public class ProductRepository : RepositoryBase<Product, Guid>, IProductRepository
    {
        private readonly IMapper _mapper;

        public ProductRepository(TPBlogContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;

        }

        public async Task CreateProductAsync(Product product)

        {
            if (await IsSlugAlreadyExisted(product.Slug))
            {
                throw new Exception("đã tồn tại slug");
            }
            var category = await _context.ProductCategories.FirstOrDefaultAsync(x => x.Id == product.ProCategoryId);
            //FindAsync(product.ProCategoryId);
            if (category == null)
            {
                throw new Exception("không tồn tại Category");
            }
            product.ProCategoryId = category.Id;
            product.ProCategoryName = category.Name;
            product.ProCategorySlug = category.Slug;
            product.DateCreated = DateTimeOffset.Now;
            product.Id = Guid.NewGuid();
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        private Task<bool> IsSlugAlreadyExisted(string slug, Guid? currentId = null)
        {
            if (currentId.HasValue)
            {
                return _context.Products.AnyAsync(x => x.Slug == slug && x.Id != currentId.Value);
            }
            return _context.Products.AnyAsync(x => x.Slug == slug);
        }


        public async Task DeleteProductAsync(Guid id)
        {
            var item = await _context.Products.FindAsync(id);
            if (item == null)
            {
                throw new Exception("Không tồn tại Item");
            }
            _context.Products.Remove(item);
            _context.SaveChanges();
        }


        public async Task<PageResult<ProductInListDto>> GetAllProductPaging(string? keyword, int pageIndex = 1, int pageSize = 10)
        {
            //var query = _context.Products.AsNoTracking();

            var query = _context.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }

            var totalRow = await query.CountAsync();

            query = query.OrderByDescending(x => x.DateCreated)
               .Skip((pageIndex - 1) * pageSize)
               .Take(pageSize);

            return new PageResult<ProductInListDto>
            {
                Results = await _mapper.ProjectTo<ProductInListDto>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };
        }

        public async Task<Product> GetProductAsync(Guid id)
       => await _context.Products.FirstOrDefaultAsync();

        public async Task<IEnumerable<Product>> GetProductByNoAsync(string productNo)
              => await _context.Products.Where(x => x.No == productNo).ToListAsync();


        public async Task<IEnumerable<Product>> GetProductsAsync()
        => await _context.Products.ToListAsync();

        public async Task UpdateProductAsync(Product product)
        {

            var item = await _context.Products.FirstOrDefaultAsync(x => x.Id == product.Id);
            if (item == null)
            {
                throw new Exception("Không tồn tại Item");
            }
            if (await IsSlugAlreadyExisted(product.Slug))
            {
                throw new Exception("đã tồn tại slug");
            }
            if (item.ProCategoryId != product.ProCategoryId)
            {
                var category = await _context.ProductCategories.Where(x => x.Id == product.ProCategoryId).FirstOrDefaultAsync();
                item.ProCategoryName = category.Name;
                item.ProCategorySlug = category.Slug;
                item.DateLastModified = DateTimeOffset.Now;
            }
            _context.Products.Update(item);
            _context.SaveChanges();
            //return item;
        }
    }
}
