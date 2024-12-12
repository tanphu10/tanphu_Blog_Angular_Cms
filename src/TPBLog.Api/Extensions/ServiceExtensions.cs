﻿using TPBlog.Core.Shared.Contracts;
using TPBlog.Data.Shared.Contracts;

namespace TPBlog.Api.Extensions
{
    public static class ServiceExtensions
    {
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
       IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection(nameof(SMTPEmailSetting))
                .Get<SMTPEmailSetting>();
            services.AddSingleton(emailSettings);
            return services;
        }
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
          => services.AddTransient<IScheduleJobService, ScheduleJobService>().AddTransient<ISmtpEmailService, SmtpEmailService>()
          .AddTransient<IBackgroundJobService, BackgroundJobService>().AddHttpClient();
    }

}