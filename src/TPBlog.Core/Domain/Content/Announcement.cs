﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Identity;

namespace TPBlog.Core.Domain.Content
{
    [Table("Announcements")]
    public class Announcement
    {
        public Announcement()
        {
            //AnnouncementUsers = new List<AnnouncementUser>();
        }
        public int Id { set; get; }

        [Required]
        public string Title { set; get; }

        public string Content { set; get; }

        public DateTimeOffset DateCreated { get; set; }

        public Guid UserId { set; get; }

        //public virtual AppUser? AppUser { get; set; }

        public bool Status { get; set; }

        //public virtual ICollection<AnnouncementUser>? AnnouncementUsers { get; set; }

    }
}