using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TPBlog.Core.Models.content;

namespace TPBlog.Api.SignalR
{
    [Authorize]
    public class NotificationsHub : Hub<INotificationClient>
    {
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
        private readonly IHubContext<NotificationsHub> _hubContext;

        public NotificationsHub(IHubContext<NotificationsHub> hubContext)
        {
            _hubContext = hubContext;
            //_connections = connections;
        }

        public async Task PushToAllUsers(AnnouncementViewModel message)
        {
            //IHubConnectionContext<dynamic> clients = GetClients(hub);
            await _hubContext.Clients.All.SendAsync("addAnnouncement", message);
        }

        public async void PushToUser(string who, AnnouncementViewModel message)
        {
            var clients = GetClients();
            foreach (var connectionId in _connections.GetConnections(who))
            {
                clients.Client(connectionId).SendAsync("addChatMessage", message);
            }
        }
        public void PushToUsers(string[] whos, AnnouncementViewModel message)
        {
            var clients = GetClients();

            for (int i = 0; i < whos.Length; i++)
            {
                var who = whos[i];
                foreach (var connectionId in _connections.GetConnections(who))
                {
                    clients.Client(connectionId).SendAsync("addChatMessage", message);
                }
            }

        }
        private IHubClients GetClients()
        {
            return _hubContext.Clients;
        }
        public override Task OnConnectedAsync()
        {
            var userName = Context.User?.Identity?.Name;
            var connectionId = Context.ConnectionId;

            if (userName != null)
            {
                if (!_connections.GetConnections(userName).Contains(connectionId))
                {
                    _connections.Add(userName, connectionId);
                }
            }

            return base.OnConnectedAsync();

        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userName = Context.User?.Identity?.Name;
            var connectionId = Context.ConnectionId;

            if (userName != null)
            {
                _connections.Remove(userName, connectionId);
            }

            return base.OnDisconnectedAsync(exception);
        }

    }
    public interface INotificationClient
    {
        Task ReceiveNotification(string message);
    }
}
