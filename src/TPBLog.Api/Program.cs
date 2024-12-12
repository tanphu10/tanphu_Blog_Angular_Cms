using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using TPBlog.Api;
using TPBlog.Core.Models.content;
using TPBlog.Data.Repositories;
using TPBlog.Data.SeedWorks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TPBlog.Api.Authorization;
using Microsoft.OpenApi.Models;
using TPBlog.Api.SignalR;
using TPBlog.Api.Extensions;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //                      .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

        var configuration = builder.Configuration;
        builder.Host.AddAppConfigurations();
        builder.Services.AddConfigurationSettings(builder.Configuration);
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.ConfigureServices();
        builder.Services.AddHangfireService();
        builder.Services.AddSignalR();

        //var connectionString = configuration.GetConnectionString("DefaultConnection");
        //configurate Cors;
        var TeduCorsPolicy = "TeduCorsPolicy";
        //builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        //builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();


        builder.Services.AddCors(o => o.AddPolicy(TeduCorsPolicy, builder =>
        {
            builder.AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins(configuration["AllowedOrigins"])
                .AllowCredentials();
        }));
        //Config DB Context and ASP.NET Core Identity
        //builder.Services.AddDbContext<TPBlogContext>(options =>
        //                 options.UseSqlServer(connectionString, b => b.MigrationsAssembly("TPBlog.Data")));
        //builder.Services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false)
        //    .AddEntityFrameworkStores<TPBlogContext>();


        //builder.Services.Configure<IdentityOptions>(options =>
        //{
        //    // Password settings.
        //    options.Password.RequireDigit = true;
        //    options.Password.RequireLowercase = true;
        //    options.Password.RequireNonAlphanumeric = true;
        //    options.Password.RequireUppercase = true;
        //    options.Password.RequiredLength = 6;
        //    options.Password.RequiredUniqueChars = 1;
        //    // Lockout settings.
        //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        //    options.Lockout.MaxFailedAccessAttempts = 5;
        //    options.Lockout.AllowedForNewUsers = false;
        //    // User settings.
        //    options.User.AllowedUserNameCharacters =
        //    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        //    options.User.RequireUniqueEmail = true;
        //});

        // Add services to the container.
        //builder.Services.AddScoped(typeof(IRepository<,>), typeof(RepositoryBase<,>));
        //builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Business services and repositories
        var services = typeof(PostRepository).Assembly.GetTypes()
            .Where(x => x.GetInterfaces().Any(i => i.Name == typeof(IRepository<,>).Name)
            && !x.IsAbstract && x.IsClass && !x.IsGenericType);

        foreach (var service in services)
        {
            var allInterfaces = service.GetInterfaces();
            var directInterface = allInterfaces.Except(allInterfaces.SelectMany(t => t.GetInterfaces())).FirstOrDefault();
            if (directInterface != null)
            {
                builder.Services.Add(new ServiceDescriptor(directInterface, service, ServiceLifetime.Scoped));
            }
        }

        //Auto mapper
        builder.Services.AddAutoMapper(typeof(PostInListDto));

        ////Authen and author
        //builder.Services.Configure<JwtTokenSettings>(configuration.GetSection("JwtTokenSettings"));
        //builder.Services.Configure<MediaSettings>(configuration.GetSection("MediaSettings"));

        //builder.Services.AddScoped<NotificationsHub>();

        //builder.Services.AddScoped<SignInManager<AppUser>, SignInManager<AppUser>>();
        //builder.Services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
        //builder.Services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();
        //builder.Services.AddScoped<ITokenService, TokenService>();
        //builder.Services.AddScoped<IPostService, PostService>();
        //builder.Services.AddScoped<IPermissionService, PermissionService>();
        //builder.Services.AddScoped<IRoyaltyService, RoyaltyService>();
        //builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
        //builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();

        builder.Services.AddHttpContextAccessor();
        //Default config for ASP.NET Core
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.CustomOperationIds(apiDesc =>
            {
                return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
            });
            c.SwaggerDoc("AdminAPI", new OpenApiInfo
            {
                Version = "v1",
                Title = "API for Administrators",
                Description = "API for CMS core domain. This domain keeps track of campaigns, campaign rules, and campaign execution."
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
            });
            c.ParameterFilter<SwaggerNullableParameterFilter>();
        });

        builder.Services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;

            cfg.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["JwtTokenSettings:Issuer"],
                ValidAudience = configuration["JwtTokenSettings:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtTokenSettings:Key"]))
            };
        });
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment() )
        //{
        app.UseSwagger(c =>
        {
            c.PreSerializeFilters.Add((document, request) =>
            {
                var paths = document.Paths.ToDictionary(item => item.Key.ToLowerInvariant(), item => item.Value);
                document.Paths.Clear();
                foreach (var pathItem in paths)
                {
                    document.Paths.Add(pathItem.Key, pathItem.Value);
                }
            });
        });
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("AdminAPI/swagger.json", "Admin API");
            c.DisplayOperationId();
            c.DisplayRequestDuration();
        });
        //}
        app.UseStaticFiles();

        app.UseCors(TeduCorsPolicy);
        app.UseMiddleware<QueryStringAuthProvider>();  // Thêm middleware tùy chỉnh vào pipeline

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();
        app.UseHangfireDashboard(builder.Configuration);

        app.MapControllers();

        //Seeding data
        app.MigrateDatabase();
        app.MapHub<NotificationsHub>("/notificationsHub");

        app.Run();
    }
}