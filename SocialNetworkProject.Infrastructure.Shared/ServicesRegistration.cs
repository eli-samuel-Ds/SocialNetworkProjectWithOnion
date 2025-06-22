using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNetworkProject.Core.Application.Interfaces;
using SocialNetworkProject.Core.Domain.Settings;
using SocialNetworkProject.Infrastructure.Shared.Services;

namespace SocialNetworkProject.Infrastructure.Shared
{
    public static class ServicesRegistration
    {
        public static void AddSharedLayerIoc(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MailSettings>(config.GetSection("MailSettings"));

            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
