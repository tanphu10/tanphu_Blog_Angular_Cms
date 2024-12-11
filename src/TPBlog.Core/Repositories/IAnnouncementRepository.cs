using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Core.Repositories
{
    public interface IAnnouncementRepository : IRepository<IC_Announcement, int>
    {
        Task<PageResult<AnnouncementViewModel>> GetUserAnnoucenmentsAsync(Guid userId);
        Task<PageResult<AnnouncementViewModel>> GetAllPaging(int pageIndex = 1, int pageSize = 10);
        Task<PageResult<AnnouncementViewModel>> ListAllUnread(Guid userId, int pageIndex=1, int pageSize=10);

    }
}
