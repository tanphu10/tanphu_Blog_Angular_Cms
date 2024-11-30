using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TPBlog.Api.Services.IServices;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Domain.Identity;
using TPBlog.Core.Models.content;
using TPBlog.Data;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Api.Services
{
    public class InventoryService : RepositoryBase<InventoryEntry, Guid>, IInventoryService
    {
        private readonly IMapper _mapper;

        public InventoryService(TPBlogContext context, IMapper mapper
           ) : base(context)
        {
            _mapper = mapper;

        }

        public async Task DeleteByDocumentNoAsync(string documentNo)
        {
            //FilterDefinition<InventoryEntry> filter = Builders<InventoryEntry>.Filter.Eq(s => s.DocumentNo, documentNo);
            var item = await _context.Inventories.FindAsync(documentNo);
            if (item == null)
            {
                throw new Exception("Không tồn tại Item");
            }
            //await Collection.DeleteManyAsync(filter);
            _context.Inventories.Remove(item);
            _context.SaveChanges();
        }

        public async Task<List<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo)
        {
            var entities = await _context.Inventories.AnyAsync(x => x.ItemNo == itemNo);
            var result = _mapper.Map<List<InventoryEntryDto>>(entities);
            return result;
        }


        public async Task<InventoryEntryDto> GetByIdAsync(Guid id)
        {
            var entity = await _context.Inventories.FirstOrDefaultAsync(x => x.Id == id);
            var result = _mapper.Map<InventoryEntryDto>(entity);
            return result;
        }


        public async Task<InventoryEntryDto> PurchaseItemAsync(string itemNo, PurchaseProductDto model)
        {
            var category = await _context.InventoryCategories.FirstOrDefaultAsync(x => x.Id == model.InvtCategoryId);
            if (category == null)
            {
                throw new Exception("không tồn tại InventoryCategory");
            }
            var itemToAdd = new InventoryEntry()
            {
                Id = Guid.NewGuid(),
                ItemNo = itemNo,
                Quantity = model.Quantity,
                DocumentType = model.DocumentType,
                InvtCategorySlug = category.Slug,
                InvtCategoryName = category.Name,
                InvtCategoryId = category.Id,
                Slug=model.Slug
            };
            await _context.Inventories.AddAsync(itemToAdd);
            var result = _mapper.Map<InventoryEntryDto>(itemToAdd);
            return result;

        }

        public async Task<InventoryEntryDto> SalesItemAsync(string itemNo, SalesProductDto model)
        {
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
                Quantity = model.Quantity,
                DocumentType = model.DocumentType,
                InvtCategorySlug = category.Slug,
                InvtCategoryName = category.Name,
                InvtCategoryId = category.Id,
                Slug = model.Slug

            };

            await _context.Inventories.AddAsync(itemToAdd);
            var result = _mapper.Map<InventoryEntryDto>(itemToAdd);
            return result;
        }

        public async Task<string> SalesOrderAsync(SalesOrderDto model)
        {
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
            return documentNo;
        }
    }
}
