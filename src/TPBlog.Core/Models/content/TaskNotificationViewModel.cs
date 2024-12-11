using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;

namespace TPBlog.Core.Models.content
{
    public class TaskNotificationViewModel
    {
        public Guid TaskId { get; set; }
        public Guid UserBy { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public string ProjectSlug { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public bool HasRead { get; set; }
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<IC_TaskNotifications, TaskNotificationViewModel>();
            }
        }
    }
}
