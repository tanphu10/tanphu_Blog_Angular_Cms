using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBlog.Core.Shared.Contracts
{
    public interface IEmailService<T> where T : class
    {
        Task SendEmailAsync(T request, CancellationToken cancellationToken = new CancellationToken());
        void SendEmail(T request);
    }
}
