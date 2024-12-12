using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TPBlog.Api.Authorization;
using TPBlog.Api.Services.IServices;
using TPBlog.Api.Services;
using TPBlog.Api.SignalR;
using TPBlog.Core.ConfigureOptions;
using TPBlog.Core.Domain.Identity;
using TPBlog.Core.Repositories;
using TPBlog.Core.Services;
using TPBlog.Data.Repositories;
using TPBlog.Data.SeedWorks;
using TPBlog.Data.Services;
using TPBlog.Data;
using Microsoft.EntityFrameworkCore;
using TPBlog.Core.Models.content;

namespace TPBlog.Api.Extensions
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(PostInListDto));

            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddScoped(typeof(IRepository<,>), typeof(RepositoryBase<,>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //Authen and author
            services.Configure<JwtTokenSettings>(configuration.GetSection("JwtTokenSettings"));
            services.Configure<MediaSettings>(configuration.GetSection("MediaSettings"));

            services.AddScoped<NotificationsHub>();

            services.AddScoped<SignInManager<AppUser>, SignInManager<AppUser>>();
            services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRoyaltyService, RoyaltyService>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IAnnouncementService, AnnouncementService>();

            services.AddDbContext<TPBlogContext>(options =>
                       options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("TPBlog.Data")));
            services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false)
                  .AddEntityFrameworkStores<TPBlogContext>();


            services.Configure<IdentityOptions>(options =>
              {
                  // Password settings.
                  options.Password.RequireDigit = true;
                  options.Password.RequireLowercase = true;
                  options.Password.RequireNonAlphanumeric = true;
                  options.Password.RequireUppercase = true;
                  options.Password.RequiredLength = 6;
                  options.Password.RequiredUniqueChars = 1;
                  // Lockout settings.
                  options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                  options.Lockout.MaxFailedAccessAttempts = 5;
                  options.Lockout.AllowedForNewUsers = false;
                  // User settings.
                  options.User.AllowedUserNameCharacters =
                  "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                  options.User.RequireUniqueEmail = true;
              });
            return services;

        }
    }
}
