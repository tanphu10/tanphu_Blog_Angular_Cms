using Microsoft.AspNetCore.Identity;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Domain.Identity;
using TPBlog.Core.Shared.Enums;

namespace TPBlog.Data
{
    public class DataSeeder
    {
        public async Task SeedAsync(TPBlogContext context)
        {
            var passwordHasher = new PasswordHasher<AppUser>();
            var rootAdminRoleId = Guid.NewGuid();
            if (!context.Roles.Any())
            {
                await context.Roles.AddAsync(new AppRole()
                {
                    Id = rootAdminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    DisplayName = "Quản Trị Viên"
                });
                await context.SaveChangesAsync();

            }
            if (!context.Users.Any())
            {
                var userId = Guid.NewGuid();
                var user = new AppUser()
                {
                    Id = userId,
                    FirstName = "Phu",
                    LastName = "Tan",
                    Email = "admin@tedu.com.vn",
                    NormalizedEmail = "ADMIN@TEDU.COM.VN",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    IsActive = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    LockoutEnabled = false,
                    DateCreated = DateTime.Now
                };
                user.PasswordHash = passwordHasher.HashPassword(user, "Admin@123$");
                await context.Users.AddAsync(user);
                await context.UserRoles.AddAsync(new IdentityUserRole<Guid>()
                {
                    RoleId = rootAdminRoleId,
                    UserId = userId,
                });
                await context.SaveChangesAsync();
            }


            //if (!context.Inventories.Any())
            //{
            //    var entity = new List<InventoryEntry>
            //    {
            //        new()
            //        {
            //            Quantity=10,
            //            DocumentNo=Guid.NewGuid().ToString(),
            //            ItemNo="Lotus",
            //            ExternalDocumentNo=Guid.NewGuid().ToString(),
            //            DocumentType=EDocumentType.Purchase,
            //            Notice="Ghi Chú Tồn Kho 1"
                        
            //        },
            //         new()
            //        {
            //            Quantity=10,
            //            DocumentNo=Guid.NewGuid().ToString(),
            //            ItemNo="Cadillac",
            //            ExternalDocumentNo=Guid.NewGuid().ToString(),
            //            DocumentType=EDocumentType.Purchase,
            //            Notice="Ghi Chú Tồn Kho 2",


            //        },
            //    };
            //    await context.Inventories.AddRangeAsync(entity);
            //    await context.SaveChangesAsync();

            //}
        }
    }
}
