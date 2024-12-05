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
        public DbSet<IC_Project> Project { get; set; }
        public DbSet<IC_PostInProject> PostInProject { get; set; }
        public DbSet<IC_Post> Posts { get; set; }
        public DbSet<IC_PostCategory> PostCategories { get; set; }
        public DbSet<IC_PostTag> PostTags { get; set; }
        public DbSet<IC_Tag> Tags { get; set; }
        public DbSet<IC_PostActivityLog> PostActivityLogs { get; set; }
        public DbSet<IC_Series> Series { get; set; }
        public DbSet<IC_Transaction> Transactions { get; set; }
        public DbSet<IC_PostInSeries> PostInSeries { get; set; }
  

        public DbSet<IC_Announcement> Announcements { get; set; }
        public DbSet<IC_AnnouncementUser> AnnouncementUsers { get; set; }


        //-- Quản lí sản phẩm và tồn kho
        public DbSet<IC_InventoryEntry> Inventories { get; set; }
        public DbSet<IC_InventoryCategory> InventoryCategories { get; set; }
        public DbSet<IC_Product> Products { get; set; }
        public DbSet<IC_ProductCategory> ProductCategories { get; set; }
        public DbSet<IC_UnitConversion> UnitConversions { get; set; }
       

        //- Quản lí Task
        public DbSet<IC_Task> Tasks { get; set; }
        public DbSet<IC_TaskComment> TaskComments { get; set; }
        public DbSet<IC_TaskAttachment> TaskAttachments { get; set; }
        public DbSet<IC_TaskTag> TaskTags { get; set; }
        public DbSet<IC_TaskUser> TaskUsers { get; set; }
        public DbSet<IC_TaskHistory> TaskHistories { get; set; }


        //  đây là phần ghi đè 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);

            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims").HasKey(x => x.Id);

            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.RoleId, x.UserId });

            builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => new { x.UserId });

            builder.Entity<IC_AnnouncementUser>().ToTable("IC_AnnouncementUsers").HasKey(x => new { x.UserId, x.AnnouncementId });
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
