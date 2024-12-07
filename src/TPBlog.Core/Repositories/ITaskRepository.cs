using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;

using TPBlog.Data.SeedWorks;

namespace TPBlog.Core.Repositories
{
    public interface ITaskRepository :  IRepository<IC_Task, Guid>
    {
        Task<bool> CreateTaskAsync(CreateUpdateTaskRequest request);
        Task<bool> UpdateTaskAsync(Guid id, CreateUpdateTaskRequest request);
        Task<bool> AssignToUserAsync(Guid id, AssignToUserRequest request);

        Task<PageResult<TaskInListDto>> GetAllPagingAsync(string? keyword, Guid? projectId, DateTime? fromDate, DateTime? toDate, int pageIndex, int pageSize);
        Task<PageResult<TaskInListDto>> GetUserTaskPagingAsync(string? keyword, Guid? userId, DateTime? fromDate, DateTime? toDate, int pageIndex, int pageSize);

        Task<bool> ValidateUserInTask(Guid TaskId);
    }
}
