using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBlog.Core.Events.RegisterSuccessed
{
    public class RegisterSuccessedEventHandler : INotificationHandler<RegisterSuccessedEvent>
    {
        public Task Handle(RegisterSuccessedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
