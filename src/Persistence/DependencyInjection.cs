using Application.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Common;
using Persistence.Development;
using Persistence.Production;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            if (isDevelopment)
            {
                services.AddDbContext<DevelopmentDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                );
                services.AddScoped<IHotelReservationContext>(provider => provider.GetService<DevelopmentDbContext>());
            }
            else
            {
                services.AddDbContext<ProductionDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                );
                services.AddScoped<IHotelReservationContext>(provider => provider.GetService<ProductionDbContext>());
            }

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IHotelRoomRepository, HotelRoomRepository>();
            services.AddScoped<IHotelRoomImageRepository, HotelRoomImageRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();

            return services;
        }
    }
}
