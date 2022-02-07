using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Infra.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Infra.Services.Email;
using Core.Sms;
using Application.Users;
using Application.Common.Interfaces;
using DNTCaptcha.Core;
using Infra.Services;
using Infra.Sms;
using Infra.Email;
using Infra.ReportingData;
using Core.ReportingData;

namespace Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        if (environment.IsEnvironment("Testing"))
        {
            // Add Persistence Infra
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "AppDb"));
        }
        else
        {
            // Add Persistence Infra
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection")));
        }

        services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());

        services.AddScoped<IDomainEventService, DomainEventService>();

        // add identity framework on top of entity framework - https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-5.0#custom-user-data
        services
            .AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 2;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        int sessionCookieLifetimeMins = configuration.GetValue("SessionCookieLifetimeMinutes", 60);

        services.ConfigureApplicationCookie(options =>
        {
            // configure login path for return urls
            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-5.0#cookie-settings
            options.LoginPath = "/Identity/Account/Login";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetimeMins);
        });

        // add email settings from app config
        IdentityInit identityInit = new();
        configuration.Bind("IdentityInit", identityInit);
        services.AddSingleton(identityInit);

        // add email settings from app config
        EmailConfiguration emailConfig = new();
        configuration.Bind("EmailSettings", emailConfig);
        services.AddSingleton(emailConfig);

        // add sms settings from app config
        SmsConfiguration smsConfig = new();
        configuration.Bind("SmsSettings", smsConfig);
        services.AddSingleton(smsConfig);

        // Add Infra services
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<ISmsSender, SmsSender>();
        services.AddTransient<IReportingDataService, ReportingDataService>();

        services.AddDNTCaptcha(options =>
        {
            // options.UseSessionStorageProvider(); // -> It doesn't rely on the server or client's times. Also it's the safest one.
            // options.UseMemoryCacheStorageProvider(); // -> It relies on the server's times. It's safer than the CookieStorageProvider.
            options.UseCookieStorageProvider() // -> It relies on the server and client's times. It's ideal for scalability, because it doesn't save anything in the server's memory.
        .ShowThousandsSeparators(false)
        .WithEncryptionKey(Guid.NewGuid().ToString());
            // options.UseDistributedCacheStorageProvider(); // --> It's ideal for scalability using `services.AddStackExchangeRedisCache()` for instance.
            // options.UseDistributedSerializationProvider();
        });

        return services;
    }
}
