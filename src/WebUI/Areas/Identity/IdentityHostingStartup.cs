using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(WebUI.Areas.Identity.IdentityHostingStartup))]

namespace WebUI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}