using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Models.Royalty;

namespace TPBlog.Core.Services
{
    public interface IRoyaltyService:IServiceScope
    {
        Task<List<RoyaltyReportByUserDto>> GetRoyaltyReportByUserAsync(Guid? userId, int fromMonth, int fromYear, int toMonth, int toYear);
        Task<List<RoyaltyReportByMonthDto>> GetRoyaltyReportByMonthAsync(Guid? userId, int fromMonth, int fromYear, int toMonth, int toYear);
        Task PayRoyaltyForUserAsync(Guid fromUserId, Guid toUserId);
    }
}
