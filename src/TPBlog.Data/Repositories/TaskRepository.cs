using AutoMapper;
using Azure.Core;
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

        public async Task<bool> AssignToUserAsync(Guid id, AssignToUserRequest request)
        {

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    //var project = _context.Project.Where(x => x.Slug == request.ProjectSlug).FirstOrDefaultAsync();
                    //if (project == null)
                    //{
                    //    throw new Exception("Project don't exist");
                    //}
                    ////var task = await _uniOfWork.IC_Tasks.GetByIdAsync(id);
                    //var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

                    //task.DateLastModified = DateTimeOffset.Now;
                    //var updateTask = _mapper.Map(request, task);

                    //// Add task
                    //_context.Tasks.Update(updateTask);
                    if (request.AssignedToUser != null && request.AssignedToUser.Length > 0)
                    {

                        //1. kiểm tra xem có bao nhiêu thèn trong TaskUsers
                        var existingAssignedUsers = await _context.TaskUsers
                            .Where(x => x.TaskId == id)
                            .ToListAsync();

                        //2.xem thử thèn gửi lên có trong array có sẵn không - nếu không thì xóa
                        foreach (var existingUser in existingAssignedUsers)
                        {
                            if (!request.AssignedToUser.Contains(existingUser.UserId))
                            {
                                _context.TaskUsers.Remove(existingUser);
                            }
                        }
                        foreach (var userId in request.AssignedToUser)
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
                    throw;
                }
            }
        }

        public async Task<bool> CreateTaskAsync(CreateUpdateTaskRequest request)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var project = await _context.Project.Where(x => x.Slug == request.ProjectSlug).FirstOrDefaultAsync();
                    if (project == null)
                    {
                        throw new Exception("Project don't exist");
                    }
                    // Map request to IC_Task entity
                    var task = _mapper.Map<CreateUpdateTaskRequest, IC_Task>(request);
                    task.DateCreated = DateTimeOffset.Now;
                    task.TimeTrackingRemaining = request.OriginalEstimate - request.TimeTrackingSpent;
                    // Add task
                    _context.Tasks.Add(task);


                    //// Add assignTaskUser for each AssignedTo user
                    //if (request.AssignedTo != null && request.AssignedTo.Length > 0)
                    //{
                    //    foreach (var userId in request.AssignedTo)
                    //    {
                    //        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                    //        if (user == null)
                    //        {
                    //            throw new Exception("không tồn tại user");
                    //        }

                    //        var assignTaskUser = new IC_TaskUser
                    //        {
                    //            TaskId = task.Id, // Ensure task.Id is available
                    //            UserId = userId,
                    //        };
                    //        _context.TaskUsers.Add(assignTaskUser);
                    //    }
                    //}


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
            var project = await _context.Project.Where(x => x.Id == projectId).FirstOrDefaultAsync();
            var query = from i in _context.Tasks
                        join p in _context.Project on i.ProjectSlug equals p.Slug into projectGroup
                        from pg in projectGroup.DefaultIfEmpty()
                        join u in _context.Users on i.UserId equals u.Id into userGroup
                        from ug in userGroup.DefaultIfEmpty()
                        join tu in _context.TaskUsers on i.Id equals tu.TaskId into taskUserGroup
                        from tug in taskUserGroup.DefaultIfEmpty()
                        join assignedUser in _context.Users on tug.UserId equals assignedUser.Id into assignedUserGroup
                        from aug in assignedUserGroup.DefaultIfEmpty()
                        group aug by new
                        {
                            i.Id,
                            i.Name,
                            i.Description,
                            i.Status,
                            i.Priority,
                            i.DueDate,
                            i.Slug,
                            i.Complete,
                            i.TimeTrackingSpent,
                            i.TimeTrackingRemaining,
                            i.OriginalEstimate,
                            i.DateCreated,
                            i.DateLastModified,
                            ProjectSlug = pg != null ? pg.Slug : null, // Nếu project không tồn tại
                            //UserName = ug != null ? ug.GetFullName() : null,     // Lấy tên người tạo task
                            //UserId = ug != null ? ug.Id : (Guid?)null   // Lấy Id của người tạo task
                        } into groupedTasks
                        select new TaskInListDto
                        {
                            Id = groupedTasks.Key.Id,
                            Name = groupedTasks.Key.Name,
                            Description = groupedTasks.Key.Description,
                            UserName = groupedTasks.Key.Name, // Thay đổi nếu cần lấy tên người tạo task
                            Status = groupedTasks.Key.Status,
                            Priority = groupedTasks.Key.Priority,
                            DueDate = groupedTasks.Key.DueDate,
                            Slug = groupedTasks.Key.Slug,
                            Complete = groupedTasks.Key.Complete,
                            TimeTrackingSpent = groupedTasks.Key.TimeTrackingSpent,
                            TimeTrackingRemaining = groupedTasks.Key.TimeTrackingRemaining,
                            OriginalEstimate = groupedTasks.Key.OriginalEstimate,
                            DateCreated = groupedTasks.Key.DateCreated,
                            DateLastModified = groupedTasks.Key.DateLastModified,
                            ProjectSlug = groupedTasks.Key.ProjectSlug,
                            ListAssignedTo = groupedTasks
                            .Where(g => g != null) // Lọc bỏ các giá trị null
                            .Select(g => new UserAssignTo
                            {
                                UserId = g.Id,
                                FullName = g.GetFullName(),
                                UserName = g.UserName,
                            })
                            .ToList()
                        };
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var normalizedKeyword = TextNormalizedName.ToTextNormalizedString(keyword);
                query = query.Where(x => x.Name.Contains(normalizedKeyword));
            }
            if (projectId != null)
            {
                query = query.Where(x => x.ProjectSlug == project.Slug);
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

        public async Task<bool> UpdateTaskAsync(Guid id, CreateUpdateTaskRequest request)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    var project = await _context.Project.Where(x => x.Slug == request.ProjectSlug).FirstOrDefaultAsync();
                    if (project == null)
                    {
                        throw new Exception("Project don't exist");
                    }
                    //var task = await _uniOfWork.IC_Tasks.GetByIdAsync(id);
                    var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
                    task.TimeTrackingRemaining = request.OriginalEstimate - request.TimeTrackingSpent;
                    task.DateLastModified = DateTimeOffset.Now;
                    var updateTask = _mapper.Map(request, task);

                    // Add task
                    _context.Tasks.Update(updateTask);



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

        public async Task<bool> ValidateUserInTask(Guid TaskId)
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