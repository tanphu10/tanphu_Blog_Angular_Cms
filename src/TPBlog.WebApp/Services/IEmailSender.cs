using TPBlog.WebApp.Models;

namespace TPBlog.WebApp.Services
{
    public interface IEmailSender
    {
        Task SendEmail(EmailData emailData);
    }
}
