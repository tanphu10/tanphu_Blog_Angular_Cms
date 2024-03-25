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
        IPostRepository BaiPost { get; }
        IPostCategoryRepository PostCategories { get; }
        ISeriesRepository Series { get; }
        ITransactionRepository Transactions { get; }
        ITagRepository Tags { get; }
        IUserRepository Users { get; }
        Task<int> CompleteAsync();
    }
}
