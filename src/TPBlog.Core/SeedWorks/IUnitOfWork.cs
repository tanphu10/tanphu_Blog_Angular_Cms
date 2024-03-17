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
        ITagRepository Tags { get; }
        Task<int> CompleteAsync();
    }
}
