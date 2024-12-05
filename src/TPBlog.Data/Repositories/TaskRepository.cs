using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Helpers;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;
using TPBlog.Core.Repositories;
using TPBlog.Data.SeedWorks;
using static TPBlog.Core.SeedWorks.Contants.Permissions;

namespace TPBlog.Data.Repositories
{
    public class TaskRepository : RepositoryBase<IC_Task, Guid>, ITaskRepository
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uniOfWork;

        public TaskRepository(TPBlogContext context, IMapper mapper) : base
            (context)
        {
            _mapper = mapper;

        }

        public async Task<bool> CreateTaskWithAssignmentAsync(CreateUpdateTaskRequest request)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var project = _context.Project.Where(x => x.Slug == request.ProjectSlug).FirstOrDefaultAsync();
                    if (project == null)
                    {
                        throw new Exception("Project don't exist");
                    }
                    // Map request to IC_Task entity
                    var task = _mapper.Map<CreateUpdateTaskRequest, IC_Task>(request);
                    task.DateCreated = DateTimeOffset.Now;

                    // Add task
                    _context.Tasks.Add(task);


                    // Add assignTaskUser for each AssignedTo user
                    if (request.AssignedTo != null && request.AssignedTo.Length > 0)
                    {
                        foreach (var userId in request.AssignedTo)
                        {
                            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                            if (user == null)
                            {
                                throw new Exception("không tồn tại user");
                            }

                            var assignTaskUser = new IC_TaskUser
                            {
                                TaskId = task.Id, // Ensure task.Id is available
                                UserId = userId,
                            };
                            _context.TaskUsers.Add(assignTaskUser);
                        }
                    }


                    // Save changes
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        await transaction.CommitAsync();
                        return true;
                    }

                    await transaction.RollbackAsync();
                    return false;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw; // Re-throw the exception to be handled by the controller
                }
            }
        }

        public async Task<PageResult<TaskInListDto>> GetAllPagingAsync(string? keyword, Guid? projectId, DateTime? fromDate, DateTime? toDate, int pageIndex, int pageSize)
        {
            var project = _context.Project.Where(x => x.Id == projectId).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new Exception("Project don't exist");
            }


            var query = from i in _context.Tasks
                        join p in _context.Project on i.ProjectSlug equals p.Slug
                        join u in _context.Users on i.UserId equals u.Id
                        //where i.InvtCategorySlug == categorySlug
                        select new TaskInListDto
                        {
                            Id = i.Id,
                            Name = i.Name,
                            Description = i.Description,
                            UserName = i.Name,
                            Status = i.Status,
                            Priority = i.Priority,
                            DueDate = i.DueDate,
                            Slug = i.Slug,
                            Complete = i.Complete,
                            TimeTrackingSpent = i.TimeTrackingSpent,
                            TimeTrackingRemaining = i.TimeTrackingRemaining,
                            OriginalEstimate = i.OriginalEstimate,
                            DateCreated = i.DateCreated,
                            DateLastModified = i.DateLastModified,
                            ProjectSlug = p.Slug,
                            ProjectId = p.Id,
                        };
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var normalizedKeyword = TextNormalizedName.ToTextNormalizedString(keyword);
                query = query.Where(x => x.Name.Contains(normalizedKeyword)
                        );
                //query = query.Where(x => x.ItemNo.Contains(keyword));
            }
            if (projectId != null)
            {
                query = query.Where(x => x.ProjectId == projectId);

            }
            // Bộ lọc theo ngày tháng
            if (fromDate.HasValue)
            {
                query = query.Where(x => x.DateCreated >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(x => x.DateCreated <= toDate.Value);
            }
            var totalRow = await query.CountAsync();

            query = query.OrderByDescending(x => x.DateCreated)
               .Skip((pageIndex - 1) * pageSize)
               .Take(pageSize);

            return new PageResult<TaskInListDto>
            {
                Results = await query.ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize,
            };
        }


        public async Task<PageResult<TaskInListDto>> GetUserTaskPagingAsync(string? keyword, Guid? userId, DateTime? fromDate, DateTime? toDate, int pageIndex, int pageSize)
        {
            //var project= await _context.Project.FirstOrDefaultAsync(x=>x.Slug==)

            var query = from i in _context.Tasks
                        join t in _context.TaskUsers on i.Id equals t.TaskId
                        join u in _context.Users on i.UserId equals u.Id
                        join p in _context.Project on i.ProjectSlug equals p.Slug
                        where t.UserId == userId
                        select new TaskInListDto
                        {
                            Id = i.Id,
                            Name = i.Name,
                            Description = i.Description,
                            UserName = i.Name,
                            Status = i.Status,
                            Priority = i.Priority,
                            DueDate = i.DueDate,
                            Slug = i.Slug,
                            Complete = i.Complete,
                            TimeTrackingSpent = i.TimeTrackingSpent,
                            TimeTrackingRemaining = i.TimeTrackingRemaining,
                            OriginalEstimate = i.OriginalEstimate,
                            DateCreated = i.DateCreated,
                            DateLastModified = i.DateLastModified,
                            ProjectSlug = i.ProjectSlug,
                            ProjectId = p.Id
                        };
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var normalizedKeyword = TextNormalizedName.ToTextNormalizedString(keyword);
                query = query.Where(x => x.Name.Contains(normalizedKeyword)
                        );
                //query = query.Where(x => x.ItemNo.Contains(keyword));
            }
            // Bộ lọc theo ngày tháng
            if (fromDate.HasValue)
            {
                query = query.Where(x => x.DateCreated >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(x => x.DateCreated <= toDate.Value);
            }
            var totalRow = await query.CountAsync();

            query = query.OrderByDescending(x => x.DateCreated)
               .Skip((pageIndex - 1) * pageSize)
               .Take(pageSize);

            return new PageResult<TaskInListDto>
            {
                Results = await query.ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize,
            };
        }

        public async Task<bool> UpdateTaskWithAssignmentAsync(Guid id, CreateUpdateTaskRequest request)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    var project = _context.Project.Where(x => x.Slug == request.ProjectSlug).FirstOrDefaultAsync();
                    if (project == null)
                    {
                        throw new Exception("Project don't exist");
                    }
                    //var task = await _uniOfWork.IC_Tasks.GetByIdAsync(id);
                    var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

                    task.DateLastModified = DateTimeOffset.Now;
                    var updateTask = _mapper.Map(request, task);

                    // Add task
                    _context.Tasks.Update(updateTask);

                    // Remove TaskUsers that are not in the AssignedTo list anymore
                    // Remove TaskUsers that are not in the AssignedTo list anymore
                    if (request.AssignedTo != null && request.AssignedTo.Length > 0)
                    {

                        //1. kiểm tra xem có bao nhiêu thèn trong TaskUsers
                        var existingAssignedUsers = await _context.TaskUsers
                            .Where(x => x.TaskId == id)
                            .ToListAsync();

                        //2.xem thử thèn gửi lên có trong array có sẵn không - nếu không thì xóa
                        foreach (var existingUser in existingAssignedUsers)
                        {
                            if (!request.AssignedTo.Contains(existingUser.UserId))
                            {
                                _context.TaskUsers.Remove(existingUser);
                            }
                        }
                        foreach (var userId in request.AssignedTo)
                        {
                            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                            if (user == null)
                            {
                                throw new Exception("User does not exist");
                            }

                            var findTaskUser = await _context.TaskUsers
                                .FirstOrDefaultAsync(x => x.TaskId == id && x.UserId == userId);

                            if (findTaskUser == null)
                            {
                                findTaskUser = new IC_TaskUser { TaskId = id, UserId = userId };
                                _context.TaskUsers.Add(findTaskUser);
                            }
                            else
                            {
                                findTaskUser.UserId = userId;
                                _context.TaskUsers.Update(findTaskUser);
                            }
                        }
                    }

                    // Save changes
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        await transaction.CommitAsync();
                        return true;
                    }

                    await transaction.RollbackAsync();
                    return false;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw; // Re-throw the exception to be handled by the controller
                }
            }
        }

        public async Task<bool> ValidateUserInTask( Guid TaskId)
        {
            var query = await _context.TaskUsers.FirstOrDefaultAsync(x => x.TaskId == TaskId);
            if (query != null)
            {
                return true;

            }
            return false;
        }
    }
}