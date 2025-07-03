using Microsoft.Extensions.DependencyInjection;
using SocialNetworkProject.Core.Application.Interfaces;
using SocialNetworkProject.Core.Application.Services;
using System.Reflection;

namespace SocialNetworkProject.Core.Application
{
    public static class ServicesRegistration
    {
        public static void AddApplicationLayerIoc(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IFileUploader, FileUploaderService>();
            services.AddTransient<IFriendshipService, FriendshipService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IBattleService, BattleService>();
        }
    }
}