using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using TPBlog.Core.Domain.Content;
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

        public async Task<List<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo)
        {
            var entities = await _context.Inventories.Where(x => x.ItemNo == itemNo)
                                 .ToListAsync();
            var result = _mapper.Map<List<InventoryEntryDto>>(entities);
            return result;
        }

        public async Task<PageResult<InventoryInListDto>> GetAllByItemNoPagingAsync(string? keyword, Guid? projectId, int pageIndex = 1, int pageSize = 10)
        {

            var project = _context.Project.Where(x => x.Id == projectId);
            if (project == null)
            {
                throw new Exception("dự án không tồn tại");
            }
            var query = from i in _context.Inventories
                        join p in _context.Project on i.ProjectId equals p.Id
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
                query = query.Where(x => x.ItemNo.Contains(keyword));
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

        public async Task<int> GetStockQuantity(string itemNo)
        {
            var query = _context.Inventories.AsQueryable();

            var result = query.Where(x => x.ItemNo == itemNo).Sum(x => x.Quantity);
            return result;
        }

        public async Task<InventoryEntryDto> PurchaseItemAsync(string itemNo, PurchaseProductDto model)
        {
            var itemToAdd = new InventoryEntry()
            {
                Id = Guid.NewGuid(),
                ItemNo = itemNo,
                Quantity = model.Quantity,
                DocumentType = model.DocumentType,
                Notice = model.Notice,
                ProjectId = model.ProjectId,
                FilePdf = model.FilePdf
            };
            await _context.Inventories.AddAsync(itemToAdd);
            _context.SaveChanges();
            var result = _mapper.Map<InventoryEntryDto>(itemToAdd);
            return result;
        }

        public async Task<InventoryEntryDto> SalesItemAsync(string itemNo, SalesProductDto model)
        {

            var itemToAdd = new InventoryEntry()
            {
                Id = Guid.NewGuid(),
                ItemNo = itemNo,
                ExternalDocumentNo = model.ExternalDocumentNo,
                Quantity = model.Quantity * -1,
                DocumentType = model.DocumentType,
                Notice = model.Notice,
                ProjectId = model.ProjectId,
                FilePdf = model.FilePdf
            };

            await _context.Inventories.AddAsync(itemToAdd);
            _context.SaveChanges();
            var result = _mapper.Map<InventoryEntryDto>(itemToAdd);
            return result;
        }

        public async Task<string> SalesOrderAsync(SalesOrderDto model)
        {
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
                };
                await _context.Inventories.AddAsync(itemToAdd);
            }
            _context.SaveChanges();
            return documentNo;
        }
    }
}
