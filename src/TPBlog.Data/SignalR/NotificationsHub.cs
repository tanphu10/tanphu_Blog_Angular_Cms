//using Microsoft.AspNet.SignalR.Hubs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Domain.Identity;
using TPBlog.Core.Models.content;
using TPBlog.Data;
using static TPBlog.Core.SeedWorks.Contants.Permissions;

namespace TPBlog.Api.SignalR
{
    [Authorize]
    public class NotificationsHub : Hub<INotificationClient>
    {
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
        private readonly IHubContext<NotificationsHub> _hubContext;
        private readonly TPBlogContext _context;
        //private static readonly ConcurrentDictionary<string, string> UserConnections = new();

        public NotificationsHub(IHubContext<NotificationsHub> hubContext, TPBlogContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessageToGroup(string groupName, AnnouncementViewModel message)
        {
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
        public async void PushToUser(string who, AnnouncementViewModel message)
        {



            var clients = GetClients();
            foreach (var connectionId in _connections.GetConnections(who))
            {
                clients.Client(connectionId).SendAsync("addChatMessage", message);
            }
        }


        //public async void PushToUsers(string[] whos, AnnouncementViewModel message)
        //{
        //    var clients = GetClients();
        //    for (int i = 0; i < whos.Length; i++)
        //    {
        //        var who = whos[i];
        //        foreach (var connectionId in _connections.GetConnections(who))
        //        {
        //            clients.Client(connectionId).SendAsync("ReceiveSystemNotification", message);
        //        }

        //    }
        //    foreach (var user in whos)
        //    {
        //        var appUser = _context.Users.Where(x => x.UserName == user).FirstOrDefault();


        //        var exists = await _context.AnnouncementUsers.AnyAsync(
        //            x => x.AnnouncementId == message.Id && x.UserId == appUser.Id);
        //        if (!exists)
        //        {
        //            var announcementUser = new IC_AnnouncementUser
        //            {
        //                AnnouncementId = message.Id,
        //                UserId = appUser.Id,
        //                HasRead = false
        //            };
        //            await _context.AnnouncementUsers.AddAsync(announcementUser);
        //        }
        //    }
        //}


        public async Task PushToAllUsers(AnnouncementViewModel message)
        {
            var appUsers = await _context.Users.ToListAsync();
            if (appUsers == null || !appUsers.Any())
            {
                throw new Exception("not found user");
            }

            await _hubContext.Clients.Group("SystemNotifications").SendAsync("ReceiveSystemNotification", message);
            // Add announcement entries for each user
            var announcementUsers = appUsers
                .Where(user => !_context.AnnouncementUsers
                                        .Any(x => x.AnnouncementId == message.Id && x.UserId == user.Id))
                .Select(user => new IC_AnnouncementUser
                {
                    AnnouncementId = message.Id,
                    UserId = user.Id,
                    HasRead = false
                })
                .ToList();
            await _context.AnnouncementUsers.AddRangeAsync(announcementUsers);
            await _context.SaveChangesAsync();
        }





        //--------------------------------- Task
        public async Task AddUsersToTaskGroup(List<string> userNames, TaskNotificationViewModel resultMessage)
        {

            var userEntities = await _context.Users.Where(u => userNames.Contains(u.UserName)).ToListAsync();
            if (_hubContext.Groups == null)
            {
                throw new InvalidOperationException("Groups is not initialized.");
            }

            var taskNotifications = new List<IC_TaskNotifications>();
            foreach (var userEntity in userEntities)
            {

                //var isAssignedTask = await _context.TaskNotifications.AnyAsync(tn => tn.TaskId == resultMessage.TaskId && tn.UserName == userEntity.UserName);
                //if (!isAssignedTask)
                //{
                //    Console.WriteLine($"User {userEntity.UserName} is not assigned to task {resultMessage.TaskId}");
                //    continue;  // Nếu người dùng không được gán nhiệm vụ, bỏ qua
                //}
                var connectionIds = _connections.GetConnections(userEntity.UserName).ToList();


                if (connectionIds == null || !connectionIds.Any())
                {
                    Console.WriteLine("No active connections found.");
                }
                foreach (var connectionId in connectionIds)
                {
                    if (string.IsNullOrEmpty(connectionId))
                    {
                        Console.WriteLine("Invalid connectionId: " + connectionId);
                        continue;
                    }
                    try
                    {
                        await _hubContext.Groups.AddToGroupAsync(connectionId, resultMessage.TaskId.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding user to group: {ex.Message}");
                    }
                }
                var existingNotification = await _context.TaskNotifications
                    .FirstOrDefaultAsync(tn => tn.TaskId == resultMessage.TaskId && tn.UserName == userEntity.UserName);

                if (existingNotification != null)
                {
                    // Nếu tồn tại, cập nhật thông tin (ví dụ: nội dung hoặc thời gian tạo)
                    existingNotification.Content = $"The news: {resultMessage.Content}";
                    existingNotification.ProjectSlug = resultMessage.ProjectSlug;
                    existingNotification.DateCreated = DateTime.UtcNow;
                }
                else
                {
                    // Nếu không tồn tại, thêm mới thông báo nhiệm vụ
                    taskNotifications.Add(new IC_TaskNotifications
                    {
                        TaskId = resultMessage.TaskId,
                        UserBy = userEntity.Id,
                        Content = $"The news: {resultMessage.Content}",
                        UserName = userEntity.UserName,
                        DateCreated = DateTime.UtcNow,
                        ProjectSlug = resultMessage.ProjectSlug
                    });
                }
                // Nếu có thông báo mới, thêm vào cơ sở dữ liệu
                if (taskNotifications.Any())
                {
                    await _context.TaskNotifications.AddRangeAsync(taskNotifications);
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                //await _context.SaveChangesAsync();
            }
        }




        public async Task SendNotificationToTaskGroup(Guid taskId, TaskNotificationViewModel taskMessage)
        {
            var taskGroupUsers = await _context.TaskNotifications
                                       .Where(tg => tg.TaskId == taskId)
                                       .ToListAsync();

            // Gửi thông báo cho nhóm nhiệm vụ
            await _hubContext.Clients.Group(taskId.ToString()).SendAsync("ReceiveTaskNotification", taskMessage);
            var notifications = taskGroupUsers.Select(x => new IC_TaskNotificationUsers
            {
                TaskId = taskId,
                UserId = x.UserBy,
                UserName = x.UserName,
                HasRead = false,
                DateCreated = DateTime.UtcNow
            }).ToList();
            foreach (var notification in notifications)
            {
                var existingNotification = await _context.TaskNotificationUsers
                    .FirstOrDefaultAsync(t => t.TaskId == notification.TaskId && t.UserId == notification.UserId);

                if (existingNotification != null)
                {
                    Console.WriteLine($"Notification already exists for TaskId: {taskId} and UserId: {notification.UserId}");
                    continue;
                }
                await _context.TaskNotificationUsers.AddAsync(notification);
            }
            //await _context.SaveChangesAsync();
        }

        //--------------------------------- Common
        private IHubClients GetClients()
        {
            return _hubContext.Clients;
        }
        public override async Task OnConnectedAsync()
        {
            var userName = Context.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(userName))
            {

                _connections.Add(userName, Context.ConnectionId);
                Console.WriteLine($"User {userName} connected with Connection ID: {Context.ConnectionId}");

                await Groups.AddToGroupAsync(Context.ConnectionId, "SystemNotifications");
                // Thêm người dùng vào các nhóm nhiệm vụ (task group) mà họ đang tham gia
                var userTasks = await _context.TaskNotifications
                                              .Where(t => t.UserName == userName)
                                              .Select(t => t.TaskId.ToString())
                                              .ToListAsync();

                foreach (var taskId in userTasks)
                {
                    Console.WriteLine($"Adding user {userName} to task group: {taskId}");
                    await Groups.AddToGroupAsync(Context.ConnectionId, taskId);
                }
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userName = Context.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                // Xóa kết nối khi người dùng ngắt kết nối
                _connections.Remove(userName, Context.ConnectionId);
                Console.WriteLine($"User {userName} disconnected.");
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
    public interface INotificationClient
    {
        Task ReceiveNotification(string message);
    }
}