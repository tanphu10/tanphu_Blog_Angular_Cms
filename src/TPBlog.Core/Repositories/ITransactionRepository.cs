using TPBlog.Core.Domain.Royalty;
using TPBlog.Core.Models;
using TPBlog.Data.SeedWorks;
using TPBlog.Core.Models.Royalty;
using Microsoft.Extensions.DependencyInjection;

namespace TPBlog.Core.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction, Guid>
    {
        Task<PageResult<TransactionDto>> GetAllPaging(string? userName,
         int fromMonth, int fromYear, int toMonth, int toYear, int pageIndex = 1, int pageSize = 10);
    }
}
