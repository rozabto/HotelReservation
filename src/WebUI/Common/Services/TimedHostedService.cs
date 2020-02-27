using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
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
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer _timer;
        private AppUser _admin;

        public TimedHostedService(ILogger<TimedHostedService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(10));

            using var scope = _serviceScopeFactory.CreateScope();
            var services = scope.ServiceProvider;

            var userManager = services.GetRequiredService<IUserManager>();
            _admin = await userManager.GetUserByEmail("adminUser@hotelReservation.com");
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation("Timed Background Service is doing work.");
            using var scope = _serviceScopeFactory.CreateScope();

            var currentUser = scope.ServiceProvider.GetRequiredService<ICurrentUserService>();
            currentUser.IsAuthenticated = true;
            currentUser.User = _admin;

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new ExpiredReservationsCommand());
            await mediator.Send(new EmployeeActivityCommand());
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose() =>
            _timer?.Dispose();
    }
}
