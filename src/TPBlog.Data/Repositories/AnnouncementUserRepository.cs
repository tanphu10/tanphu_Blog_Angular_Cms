using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Repositories;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Data.Repositories
{
    public class AnnouncementUserRepository : RepositoryBase<IC_AnnouncementUser, int>, IAnnouncementUserRepository
    {
        public AnnouncementUserRepository(TPBlogContext context) : base(context)
        {
        }
    }
}
