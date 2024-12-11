using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TPBlog.Core.Domain.Entity;
using TPBlog.Core.Shared.Enums;

namespace TPBlog.Core.Domain.Content
{
    [Table("IC_TaskNotificationUsers")]
    public class IC_TaskNotificationUsers 
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public bool HasRead { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
