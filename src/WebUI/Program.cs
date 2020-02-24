using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Data.Commands.SeedData;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;

namespace WebUI
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var hotelReservationContext = services.GetRequiredService<IHotelReservationContext>();
                await hotelReservationContext.Database.MigrateAsync();

                var mediator = services.GetRequiredService<IMediator>();
                await mediator.Send(new SeedDataCommand(), CancellationToken.None);
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                webBuilder.UseStartup<Startup>()
                    .UseKestrel((_, options) =>
                    {
                        var port = Environment.GetEnvironmentVariable("PORT");
                        if (!string.IsNullOrEmpty(port))
                            options.ListenAnyIP(int.Parse(port));

                        options.Limits.MaxRequestBodySize = 10 /* Megabytes */ * 1000 /* Kilobytes */ * 1000 /* Bytes */;
                    })
            );
    }
}