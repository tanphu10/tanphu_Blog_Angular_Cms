using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Shared.Services.Email;

namespace TPBlog.Core.Shared.Contracts
{
    public interface ISmtpEmailService : IEmailService<MailRequest>
    {
    }
}
