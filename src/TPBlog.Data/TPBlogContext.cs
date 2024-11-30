using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Domain.Identity;
using TPBlog.Core.Domain.Royalty;
using TPBlog.Core.SeedWorks.Constants;

namespace TPBlog.Data
{
    public class TPBlogContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public TPBlogContext(DbContextOptions options) : base(options)
        {
        }

        //Quản lí bài viết và dự án
        public DbSet<Project> Project { get; set; }
        public DbSet<PostInProject> PostInProject { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostActivityLog> PostActivityLogs { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PostInSeries> PostInSeries { get; set; }
  

        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<AnnouncementUser> AnnouncementUsers { get; set; }


        //-- Quản lí sản phẩm và tồn kho
        public DbSet<InventoryEntry> Inventories { get; set; }
        public DbSet<InventoryCategory> InventoryCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
       

        //- Quản lí Task
        public DbSet<TbTask> Tasks { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<TaskAttachment> TaskAttachments { get; set; }
        public DbSet<TaskTag> TaskTags { get; set; }
        public DbSet<TaskHistory> TaskHistories { get; set; }


        //  đây là phần ghi đè 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);

            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims").HasKey(x => x.Id);

            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.RoleId, x.UserId });

            builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => new { x.UserId });

            builder.Entity<AnnouncementUser>().ToTable("AnnouncementUsers").HasKey(x => new { x.UserId, x.AnnouncementId });
        }
        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var entries = ChangeTracker
        //       .Entries()
        //       .Where(e => e.State == EntityState.Added);

        //    foreach (var entityEntry in entries)
        //    {
        //        var dateCreatedProp = entityEntry.Entity.GetType().GetProperty(SystemConstants.DateCreatedField);
        //        if (entityEntry.State == EntityState.Added
        //            && dateCreatedProp != null)
        //        {
        //            dateCreatedProp.SetValue(entityEntry.Entity, DateTime.Now);
        //        }
        //    }
        //    return base.SaveChangesAsync(cancellationToken);
        //}
    }
}
