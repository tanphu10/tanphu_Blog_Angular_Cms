using Microsoft.EntityFrameworkCore;
using TPBlog.Data;

namespace TPBlog.Api
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using(var scope= app.Services.CreateScope())
            {
                using(var context = scope.ServiceProvider.GetRequiredService<TPBlogContext>())
                {
                    context.Database.Migrate();
                    new DataSeeder().SeedAsync(context).Wait();

                }
                return app;
            }
        }

    }
}
