using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Api.Services.IServices
{
    public interface IAnnouncementService
    {
        Task MarkAsRead(Guid userId, int notificationId);
        Task CreateAsync(Announcement model);

    }
}
