using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SocialNetworkProject.Core.Application
{
    public static class ServicesRegistration
    {
        public static void AddApplicationLayerIoc(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

        }
    }
}