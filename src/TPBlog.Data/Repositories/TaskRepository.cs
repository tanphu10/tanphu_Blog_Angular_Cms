using AutoMapper;
using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TPBlog.Api.SignalR;
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
        private readonly NotificationsHub _notificationsHub;
        private readonly TPBlogContext _context;

        public TaskRepository(TPBlogContext context, IMapper mapper, NotificationsHub notificationsHub) : base
            (context)
        {
            _mapper = mapper;
            _notificationsHub = notificationsHub;
            _context = context;
        }
        public async Task<PageResult<TaskNotificationViewModel>> ListAllTaskUnreadAsync(Guid userId, int pageIndex = 1, int pageSize = 10)
        {

            var user = _context.Users.FirstOrDefaultAsync(x => x.Id == userId).Result;

            var query = (from x in _context.TaskNotifications
                         join y in _context.TaskNotificationUsers on x.TaskId equals y.TaskId into xy
                         from y in xy.DefaultIfEmpty()
                         where ( y.UserId == userId)
                         select new TaskNotificationViewModel
                         {
                             TaskId = x.TaskId,
                             UserBy = x.UserBy,
                             Content = x.Content,
                             UserName = x.UserName,
                             ProjectSlug = x.ProjectSlug,
                             DateCreated = x.DateCreated,
                             HasRead = y.HasRead
                         });
            var totalRow = await query.CountAsync();

            var result = await query.OrderByDescending(x => x.DateCreated)
               .Skip((pageIndex - 1) * pageSize)
               .Take(pageSize).ToListAsync();
            return new PageResult<TaskNotificationViewModel>
            {
                Results = result,
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };





        }

        public async Task<PageResult<TaskNotificationViewModel>> GetUserTaskNotificationAsync(Guid userId)
        {
            var query = await (from au in _context.TaskNotificationUsers
                               join a in _context.TaskNotifications on au.TaskId equals a.TaskId
                               where au.UserId == userId && !au.HasRead
                               select new TaskNotificationViewModel
                               {
                                   TaskId = a.TaskId,
                                   UserName = a.UserName,
                                   UserBy = a.UserBy,
                                   Content = a.Content,
                                   DateCreated = au.DateCreated,
                                   ProjectSlug = a.ProjectSlug
                               }).ToListAsync();
            return new PageResult<TaskNotificationViewModel>
            {
                Results = query.ToList(),
            };
        }
        public async Task MarkTaskAsReadAsync(Guid userId, Guid notificationId)
        {
            var announ = await _context.TaskNotificationUsers.Where(x => x.TaskId == notificationId && x.UserId == userId).FirstOrDefaultAsync();
            var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            if (announ == null)
            {
                _context.TaskNotificationUsers.Add(new IC_TaskNotificationUsers()
                {
                    TaskId = notificationId,
                    UserId = userId,
                    UserName = user.UserName,
                    HasRead = true,
                    DateCreated = DateTime.UtcNow
                });
            }
            else
            {
                announ.HasRead = true;
            }
            await _context.SaveChangesAsync(); // Hoặc _context.SaveChangesAsync();
        }

        public async Task<bool> AssignToUserAsync(Guid taskId, AssignToUserRequest request)
        {

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 1. Lấy danh sách người dùng hiện tại
                    var existingAssignedUsers = await _context.TaskUsers
                        .Where(x => x.TaskId == taskId)
                        .ToListAsync();
                    var existingUserIds = existingAssignedUsers.Select(x => x.UserId).ToHashSet();
                    var newUserIds = request.AssignedToUser.ToHashSet();

                    // 2. Xóa người dùng không còn trong danh sách mới
                    var usersToRemove = existingUserIds.Except(newUserIds).ToList();
                    if (usersToRemove.Any())
                    {
                        var taskUsersToRemove = existingAssignedUsers
                            .Where(x => usersToRemove.Contains(x.UserId))
                            .ToList();

                        _context.TaskUsers.RemoveRange(taskUsersToRemove);
                    }

                    // 3. Thêm người dùng mới chưa có trong danh sách cũ
                    var usersToAdd = newUserIds.Except(existingUserIds).ToList();
                    if (usersToAdd.Any())
                    {
                        var validUsers = await _context.Users
                            .Where(x => usersToAdd.Contains(x.Id))
                            .Select(x => x.Id)
                            .ToListAsync();

                        if (validUsers.Count != usersToAdd.Count)
                        {
                            throw new Exception("One or more users do not exist");
                        }

                        var newTaskUsers = validUsers.Select(userId => new IC_TaskUser
                        {
                            TaskId = taskId,
                            UserId = userId
                        }).ToList();

                        _context.TaskUsers.AddRangeAsync(newTaskUsers);
                    }

                    // 4. Ghi log thay đổi
                    var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);
                    if (task == null)
                    {
                        throw new Exception("Task does not exist");
                    }

                    var taskHistory = new IC_TaskHistory
                    {
                        TaskId = task.Id,
                        UserId = task.UserId,
                        TaskSlug = task.Slug,
                        ChangeTaskStatus = task.Status,
                        OldContent = $"AssignedUserIds: {string.Join(", ", existingUserIds)}",
                        NewContent = $"AssignedUserIds: {string.Join(", ", newUserIds)}",
                        ChangeDate = DateTime.Now,
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now,
                        ProjectSlug = task.ProjectSlug
                    };

                    _context.TaskHistories.Add(taskHistory);
                    var result = await _context.SaveChangesAsync();

                    // 5. Lưu thay đổi
                    if (result > 0)
                    {
                        var userNamesToAdd = await (from taskUser in _context.TaskUsers
                                                    join user in _context.Users
                                                    on taskUser.UserId equals user.Id
                                                    where taskUser.TaskId == taskId
                                                    select user.UserName).ToListAsync();

                        // Thêm các userName vào nhóm SignalR
                        if (userNamesToAdd.Any())
                        {
                            var message = "old Content" + string.Join(", ", existingUserIds) + "; new Content" + string.Join(", ", newUserIds);

                            // Gửi thông báo tới nhóm
                            var user = await _context.Users.Where(x => x.Id == task.UserId).FirstOrDefaultAsync();
                            if (user == null)
                            {
                                throw new InvalidOperationException("user is not initialized.");
                            }
                            var taskAnnounce = new TaskNotificationViewModel
                            {
                                TaskId = task.Id,
                                Content = message,
                                UserBy = task.UserId,
                                UserName = user.UserName,
                                ProjectSlug = task.ProjectSlug,
                                DateCreated = DateTime.UtcNow,
                                HasRead = false
                            };

                            await _notificationsHub.AddUsersToTaskGroup(userNamesToAdd, taskAnnounce);

                            //var taskNotification = await _context.TaskNotifications.Where(x => x.TaskId == task.Id && x.UserBy ==task.UserId ).FirstOrDefaultAsync();
                            //var resultMessage = _mapper.Map<TaskNotificationViewModel>(taskNotification);

                            await _notificationsHub.SendNotificationToTaskGroup(taskId, taskAnnounce);
                        }
                        await transaction.CommitAsync();
                        return true;
                    }

                    await transaction.RollbackAsync();
                    return false;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // Log lỗi để tiện theo dõi
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }


            }
        }

        public async Task<bool> CreateTaskAsync(Guid userId, CreateUpdateTaskRequest request)
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
                    task.UserId = userId;
                    _context.Tasks.Add(task);


                    var taskHistory = new IC_TaskHistory()
                    {
                        TaskId = task.Id,
                        UserId = task.UserId,
                        TaskSlug = task.Slug,
                        ChangeTaskStatus = task.Status,
                        OldContent = $"Name: {task.Name}, Priority: {task.Priority}",
                        NewContent = $"Name: {task.Name}, Priority: {task.Priority}",
                        ChangeDate = DateTime.Now,
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now,
                        ProjectSlug = task.ProjectSlug
                    };

                    _context.TaskHistories.Add(taskHistory);

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
                            i.StartDate,
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
                            StartDate = groupedTasks.Key.StartDate,
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
                    // Lưu thông tin cũ để ghi log
                    var oldName = task.Name;
                    var oldPriority = task.Priority;
                    task.TimeTrackingRemaining = request.OriginalEstimate - request.TimeTrackingSpent;
                    task.DateLastModified = DateTimeOffset.Now;
                    var updateTask = _mapper.Map(request, task);

                    // Add task
                    _context.Tasks.Update(updateTask);
                    // Save changes
                    var result = await _context.SaveChangesAsync();
                    var taskHistory = new IC_TaskHistory()
                    {
                        TaskId = id,
                        UserId = task.UserId,
                        TaskSlug = task.Slug,
                        ChangeTaskStatus = task.Status,
                        OldContent = $"Name: {oldName}, Priority: {oldPriority}",
                        NewContent = $"Name: {task.Name}, Priority: {task.Priority}",
                        ChangeDate = DateTime.Now,
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now,
                        ProjectSlug = task.ProjectSlug
                    };
                    _context.TaskHistories.Add(taskHistory);


                    var taskUsers = await _context.TaskUsers.Where(x => x.TaskId == task.Id).ToListAsync(); ;
                    // Giả sử bạn có phương thức này để lấy danh sách người dùng

                    //if (taskUsers != null)
                    //{
                    //    var message = new TaskNotificationViewModel
                    //    {
                    //        Content = $"Có một task mới với nội dung: {request.Name}",
                    //    };

                    //    // Gửi thông báo tới từng người dùng trong taskUsers
                    //    foreach (var user in taskUsers)
                    //    {
                    //        // Gửi thông báo tới người dùng
                    //        var userConnection = _notificationsHub.FirstOrDefault(u => u.Id == taskUser.UserId);
                    //        if (userConnection != null)
                    //        {
                    //            string connectionId = userConnection.ConnectionId;
                    //            // Tiến hành gửi thông báo hoặc làm gì đó với connectionId
                    //        }
                    //    }
                    //}
                    // Gửi thông báo đến các người dùng liên quan trong taskUsers

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

        public async Task<PageResult<TaskInListDto>> GetAllUserTaskAsync(Guid userId)
        {

            var query = await (from i in _context.Tasks
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
                               }).ToListAsync();


            return new PageResult<TaskInListDto>
            {
                Results = query,
            };
        }


    }
}