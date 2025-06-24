using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNetworkProject.Core.Domain.Interfaces;
using SocialNetworkProject.Core.Domain.Interfaces.Generic;
using SocialNetworkProject.Infrastructure.Persistence.Contexts;
using SocialNetworkProject.Infrastructure.Persistence.Repositories;
using SocialNetworkProject.Infrastructure.Persistence.Repositories.Generic;

namespace SocialNetworkProject.Infrastructure.Persistence
{
    public static class ServicesRegistration
    {
        public static void AddPersistenceLayerIoc(this IServiceCollection services, IConfiguration config)
        {
            #region Contexts
            if (config.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<SocialNetworkProjectContext>(opt =>
                                            opt.UseInMemoryDatabase("AppDb"));
            }
            else
            {
                var connectionString = config.GetConnectionString("DefaultConnection");
                services.AddDbContext<SocialNetworkProjectContext>(
                    (serviceProvider, opt) =>
                    {
                        opt.EnableSensitiveDataLogging(); 
                        opt.UseSqlServer(connectionString,
                        m => m.MigrationsAssembly(typeof(SocialNetworkProjectContext).Assembly.FullName));
                    },
                    contextLifetime: ServiceLifetime.Scoped,
                    optionsLifetime: ServiceLifetime.Scoped
                );
            }
            #endregion

            #region Repositories IOC
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IPostReactionRepository, PostReactionRepository>();
            services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
            services.AddScoped<IFriendshipRepository, FriendshipRepository>();
            services.AddScoped<IBattleRepository, BattleRepository>();
            services.AddScoped<IAttackRepository, AttackRepository>();
            services.AddScoped<IShipRepository, ShipRepository>();
            services.AddScoped<IShipPositionRepository, ShipPositionRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            #endregion
        }
    }
}