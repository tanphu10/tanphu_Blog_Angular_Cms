using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Helpers;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;
using TPBlog.Core.Repositories;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Data.Repositories
{
    public class InventoryRepository : RepositoryBase<InventoryEntry, Guid>, IInventoryRepository
    {
        private readonly IMapper _mapper;

        public InventoryRepository(TPBlogContext context, IMapper mapper
           ) : base(context)
        {
            _mapper = mapper;

        }

        //public IServiceProvider ServiceProvider => throw new NotImplementedException();

        public async Task DeleteByDocumentNoAsync(string documentNo)
        {
            var item = await _context.Inventories.FindAsync(documentNo);
            if (item == null)
            {
                throw new Exception("Không tồn tại Item");
            }
            _context.Inventories.Remove(item);
            _context.SaveChanges();
        }
        public async Task DeleteByIdAsync(Guid id)
        {
            var item = await _context.Inventories.FindAsync(id);
            if (item == null)
            {
                throw new Exception("Không tồn tại Item");
            }
            _context.Inventories.Remove(item);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<PageResult<InventoryInListDto>> GetAllByCategoryPagingAsync(string? keyword, Guid? categoryId, Guid? projectId, int pageIndex = 1, int pageSize = 10)
        {
            var project = _context.Project.Where(x => x.Id == projectId);
            if (project == null)
            {
                throw new Exception("dự án không tồn tại");
            }
            var category = _context.InventoryCategories.Where(x => x.Id == categoryId);
            if (category == null)
            {
                throw new Exception("danh mục không tồn tại");
            }
            var query = from i in _context.Inventories
                        join p in _context.Project on i.ProjectId equals p.Id
                        join ic in _context.InventoryCategories on i.InvtCategoryId equals ic.Id
                        select new InventoryInListDto
                        {
                            Id = i.Id,
                            DocumentNo = i.DocumentNo,
                            DocumentType = i.DocumentType,
                            ItemNo = i.ItemNo,
                            Quantity = i.Quantity,
                            Thumbnail = i.Thumbnail,
                            FilePdf = i.FilePdf,
                            DateCreated = i.DateCreated,
                            DateLastModified = i.DateLastModified,
                            ProjectId = p.Id,
                            ProjectSlug = p.Slug,
                            ProjectName = p.Name,
                            InvtCategoryId = ic.Id,
                            InvtCategoryName = ic.Name,
                            InvtCategorySlug = ic.Slug

                        };


            //var query = _context.Inventories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var normalizedKeyword = TextNormalizedName.ToTextNormalizedString(keyword);
                query = query.Where(x => x.ItemNo.Contains(normalizedKeyword));
            }
            if (projectId != null)
            {
                query = query.Where(x => x.ProjectId == projectId);

            }
            var totalRow = await query.CountAsync();

            query = query.OrderByDescending(x => x.DateCreated)
               .Skip((pageIndex - 1) * pageSize)
               .Take(pageSize);

            return new PageResult<InventoryInListDto>
            {
                Results = await query.ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize,
            };
        }

        public async Task<List<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo)
        {
            var entities = await _context.Inventories.Where(x => x.ItemNo == itemNo)
                                 .ToListAsync();
            var result = _mapper.Map<List<InventoryEntryDto>>(entities);
            return result;
        }

        public async Task<PageResult<InventoryInListDto>> GetAllByItemNoPagingAsync(string? keyword, Guid? projectId, string? categorySlug, int pageIndex = 1, int pageSize = 10)
        {

            var project = _context.Project.Where(x => x.Id == projectId);
            if (project == null)
            {
                throw new Exception("Project don't exist");
            }
            var category = _context.InventoryCategories.Where(x => x.Slug == categorySlug);
            if (category == null)
            {
                throw new Exception("Category don't exist");
            }
            var query = from i in _context.Inventories
                        join p in _context.Project on i.ProjectId equals p.Id
                        join c in _context.InventoryCategories on i.InvtCategoryId equals c.Id
                        //where i.InvtCategorySlug == categorySlug
                        select new InventoryInListDto
                        {
                            Id = i.Id,
                            DocumentNo = i.DocumentNo,
                            DocumentType = i.DocumentType,
                            ItemNo = i.ItemNo,
                            Quantity = i.Quantity,
                            Thumbnail = i.Thumbnail,
                            FilePdf = i.FilePdf,
                            DateCreated = i.DateCreated,
                            DateLastModified = i.DateLastModified,
                            ProjectId = p.Id,
                            ProjectSlug = p.Slug,
                            ProjectName = p.Name

                        };
            //var query = _context.Inventories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var normalizedKeyword = TextNormalizedName.ToTextNormalizedString(keyword);
                query = query.Where(x => x.ItemNo.Contains(normalizedKeyword)
                        );
                //query = query.Where(x => x.ItemNo.Contains(keyword));
            }
            if (projectId != null)
            {
                query = query.Where(x => x.ProjectId == projectId);

            }
            var totalRow = await query.CountAsync();

            query = query.OrderByDescending(x => x.DateCreated)
               .Skip((pageIndex - 1) * pageSize)
               .Take(pageSize);

            return new PageResult<InventoryInListDto>
            {
                Results = await query.ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize,
            };
        }

        public async Task<InventoryEntryDto> GetByIdAsync(Guid id)
        {
            var entity = await _context.Inventories.FirstOrDefaultAsync(x => x.Id == id);
            var result = _mapper.Map<InventoryEntryDto>(entity);
            return result;
        }

        public async Task<InventoryEntryDto> GetBySlug(string slug)
        {
            var post = await _context.Inventories.FirstOrDefaultAsync(x => x.Slug == slug);
            if (post == null) throw new Exception($"cannot find post with slug:{slug}");
            return _mapper.Map<InventoryEntryDto>(post);
        }

        public async Task<int> GetStockQuantity(string itemNo)
        {
            var query = _context.Inventories.AsQueryable();

            var result = query.Where(x => x.ItemNo == itemNo).Sum(x => x.Quantity);
            return result;
        }
        public Task<bool> IsSlugAlreadyExisted(string slug, Guid? currentId = null)
        {
            if (currentId.HasValue)
            {
                return _context.Inventories.AnyAsync(x => x.Slug == slug && x.Id != currentId.Value);
            }
            return _context.Inventories.AnyAsync(x => x.Slug == slug);
        }
        public async Task<InventoryEntryDto> PurchaseItemAsync(string itemNo, PurchaseProductDto model)
        {
            if (await IsSlugAlreadyExisted(model.Slug))
            {
                throw new Exception("đã tồn tại slug");

            }

            var category = await _context.InventoryCategories.FirstOrDefaultAsync(x => x.Id == model.InvtCategoryId);
            if (category == null)
            {
                throw new Exception("không tồn tại InventoryCategory");
            }
            //var post= await _context.in
            var itemToAdd = new InventoryEntry()
            {
                Id = Guid.NewGuid(),
                ItemNo = itemNo,
                Quantity = model.Quantity,
                DocumentType = model.DocumentType,
                Notice = model.Notice,
                ProjectId = model.ProjectId,
                FilePdf = model.FilePdf,
                InvtCategorySlug = category.Slug,
                InvtCategoryName = category.Name,
                InvtCategoryId = category.Id,
                Slug = model.Slug
            };
            await _context.Inventories.AddAsync(itemToAdd);
            _context.SaveChanges();
            var result = _mapper.Map<InventoryEntryDto>(itemToAdd);
            return result;
        }

        public async Task<InventoryEntryDto> SalesItemAsync(string itemNo, SalesProductDto model)
        {
            if (await IsSlugAlreadyExisted(model.Slug))
            {
                throw new Exception("đã tồn tại slug");

            }
            var category = await _context.InventoryCategories.FirstOrDefaultAsync(x => x.Id == model.InvtCategoryId);
            if (category == null)
            {
                throw new Exception("không tồn tại InventoryCategory");
            }

            var itemToAdd = new InventoryEntry()
            {
                Id = Guid.NewGuid(),
                ItemNo = itemNo,
                ExternalDocumentNo = model.ExternalDocumentNo,
                Quantity = model.Quantity * -1,
                DocumentType = model.DocumentType,
                Notice = model.Notice,
                ProjectId = model.ProjectId,
                FilePdf = model.FilePdf,
                InvtCategorySlug = category.Slug,
                InvtCategoryName = category.Name,
                InvtCategoryId = category.Id,
                Slug = model.Slug,
            };

            await _context.Inventories.AddAsync(itemToAdd);
            _context.SaveChanges();
            var result = _mapper.Map<InventoryEntryDto>(itemToAdd);
            return result;
        }

        public async Task<string> SalesOrderAsync(SalesOrderDto model)
        {
            if (await IsSlugAlreadyExisted(model.Slug))
            {
                throw new Exception("đã tồn tại slug");

            }
            var category = await _context.InventoryCategories.FirstOrDefaultAsync(x => x.Id == model.InvtCategoryId);
            if (category == null)
            {
                throw new Exception("không tồn tại InventoryCategory");
            }
            var documentNo = Guid.NewGuid().ToString();
            foreach (var saleItem in model.SaleItems)
            {
                var itemToAdd = new InventoryEntry()
                {
                    Id = Guid.NewGuid(),
                    DocumentNo = documentNo,
                    ItemNo = saleItem.ItemNo,
                    ExternalDocumentNo = model.OrderNo,
                    Quantity = saleItem.Quantity * -1,
                    DocumentType = saleItem.DocumentType,
                    InvtCategorySlug = category.Slug,
                    InvtCategoryName = category.Name,
                    InvtCategoryId = category.Id,
                    Slug = model.Slug
                };
                await _context.Inventories.AddAsync(itemToAdd);
            }
            _context.SaveChanges();
            return documentNo;
        }
    }
}
