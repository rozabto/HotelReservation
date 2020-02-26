using Application.Common.Interfaces;
using CloudinaryDotNet;
using Common;
using Domain.Entities;
using Infrastructure.Common;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Development;
using Persistence.Production;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            var account = new Account(
                configuration.GetValue<string>("Key:Cloudinary:Name"),
                configuration.GetValue<string>("Key:Cloudinary:Api_Key"),
                configuration.GetValue<string>("Key:Cloudinary:Api_Secret")
            );
            var cloudinary = new Cloudinary(account);

            services.AddSingleton(_ => cloudinary);
            services.AddSingleton<IMemoryService, MemoryService>();

            services.AddScoped<IUserManager, UserManagerService>();
            services.AddScoped<IRoleManager, RoleManagerService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IDateTime, UniversalDateTime>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<ICheckoutService, CheckoutService>();

            var identity = services.AddDefaultIdentity<AppUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 12;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
            })
                .AddRoles<AppRole>()
                .AddDefaultUI();

            if (isDevelopment)
                identity.AddEntityFrameworkStores<DevelopmentDbContext>();
            else identity.AddEntityFrameworkStores<ProductionDbContext>();

            return services;
        }
    }
}