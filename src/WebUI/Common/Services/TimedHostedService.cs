using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Data.Commands.CurrencyConversion;
using Application.Data.Commands.EmployeeActivity;
using Application.Data.Commands.ExpiredReservations;
using Common;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebUI.Common.Services
{
    internal class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<TimedHostedService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _10MinTimer;
        private Timer _hourTimer;
        private AppUser _admin;

        public TimedHostedService(ILogger<TimedHostedService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            using (var scope = _scopeFactory.CreateScope())
            {
                var services = scope.ServiceProvider;

                var userManager = services.GetRequiredService<IUserManager>();
                _admin = await userManager.GetUserByEmail("adminUser@hotelReservation.com");
            }

            _10MinTimer = new Timer(On10MinPassed, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(10));

            _hourTimer = new Timer(OnHourPassed, null, TimeSpan.Zero,
                TimeSpan.FromHours(1));
        }

        private async void On10MinPassed(object state)
        {
            _logger.LogInformation("10 Minute Timed Background Service is doing work.");
            using var scope = _scopeFactory.CreateScope();

            var currentUser = scope.ServiceProvider.GetRequiredService<ICurrentUserService>();
            currentUser.IsAuthenticated = true;
            currentUser.User = _admin;

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new ExpiredReservationsCommand());
            await mediator.Send(new EmployeeActivityCommand());
        }

        private async void OnHourPassed(object state)
        {
            _logger.LogInformation("1 Hour Timed Background Service is doing work.");
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new CurrencyConversionCommand());
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _10MinTimer?.Change(Timeout.Infinite, 0);
            _hourTimer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _10MinTimer?.Dispose();
            _hourTimer?.Dispose();
        }
    }
}
