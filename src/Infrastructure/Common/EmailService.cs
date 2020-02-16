using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;

namespace Infrastructure.Common
{
    public class EmailService : IEmailService
    {
        public Task SendAsync(EmailDto message) => Task.CompletedTask;
    }
}
