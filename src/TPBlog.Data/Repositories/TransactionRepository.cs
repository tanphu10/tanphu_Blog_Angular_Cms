using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TPBlog.Core.Models.Royalty;
using TPBlog.Core.Models;
using TPBlog.Core.Repositories;
using TPBlog.Data.SeedWorks;
using TPBlog.Core.Domain.Royalty;
using TPBlog.Core.Helpers;

namespace TPBlog.Data.Repositories
{
    public class TransactionRepository:RepositoryBase<IC_Transaction, Guid>,ITransactionRepository
    {
        private readonly IMapper _mapper;
        public TransactionRepository(TPBlogContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        public async Task<PageResult<TransactionDto>> GetAllPaging(string? userName,
        int fromMonth, int fromYear, int toMonth, int toYear, int pageIndex = 1, int pageSize = 10)
        {
            var query = _context.Transactions.AsQueryable();
            if (!string.IsNullOrWhiteSpace(userName))
            {

                var normalizedKeyword = TextNormalizedName.ToTextNormalizedString(userName);
                query = query.Where(x => x.ToUserName.Contains(normalizedKeyword));
            }
            if (fromMonth > 0 && fromYear > 0)
            {
                query = query.Where(x => x.DateCreated.Date.Month >= fromMonth && x.DateCreated.Year >= fromYear);
            }
            if (toMonth > 0 && toYear > 0)
            {
                query = query.Where(x => x.DateCreated.Date.Month <= toMonth && x.DateCreated.Year <= toYear);
            }
            var totalRow = await query.CountAsync();

            query = query.OrderByDescending(x => x.DateCreated)
               .Skip((pageIndex - 1) * pageSize)
               .Take(pageSize);

            return new PageResult<TransactionDto>
            {
                Results = await _mapper.ProjectTo<TransactionDto>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };

        }

    }
}
