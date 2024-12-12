using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBlog.Core.Shared.Contracts
{
    public interface IBackgroundJobService
    {
        IScheduleJobService scheduledJobService { get; }
        string? SendEmailContent(string email, string subject, string emailContent, DateTimeOffset enqueueAt);

        Task<List<City>> GetApiAsync();
    }
}
