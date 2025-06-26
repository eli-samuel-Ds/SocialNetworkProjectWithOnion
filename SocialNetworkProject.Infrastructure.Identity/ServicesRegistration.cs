using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNetworkProject.Core.Application.Interfaces;
using SocialNetworkProject.Infrastructure.Identity.Contexts;
using SocialNetworkProject.Infrastructure.Identity.Entities;
using SocialNetworkProject.Infrastructure.Identity.Seeds;
using SocialNetworkProject.Infrastructure.Identity.Services;

namespace SocialNetworkProject.Infrastructure.Identity
{
    public static class ServicesRegistration
    {
        public static void AddIdentityLayerIoc(this IServiceCollection services, IConfiguration config)
        {
            GeneralConfiguration(services, config);

            services.AddScoped<IAccountService, AccountService>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            });

            services.AddIdentityCore<AppUser>()
                .AddRoles<IdentityRole>()
                .AddSignInManager()
                .AddEntityFrameworkStores<IdentityContextSocial>()
                .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(12);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
            }).AddCookie(IdentityConstants.ApplicationScheme, options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(180);
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/Login/AccessDenied";
            });
        }

        public static async Task RunIdentitySeedAsync(this IServiceProvider service)
        {
            using var scope = service.CreateScope();
            var servicesProvider = scope.ServiceProvider;

            var userManager = servicesProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = servicesProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await DefaultRoles.SeedAsync(roleManager);
            await DefaultUserNormal.SeedAsync(userManager);
            await DefaultAdministradorUser.SeedAsync(userManager);
        }

        private static void GeneralConfiguration(IServiceCollection services, IConfiguration config)
        {
            if (config.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContextSocial>(opt =>
                    opt.UseInMemoryDatabase("IdentityDb"));
            }
            else
            {
                var connectionString = config.GetConnectionString("DefaultConnection");
                services.AddDbContext<IdentityContextSocial>(
                    (serviceProvider, options) =>
                    {
                        options.EnableSensitiveDataLogging();
                        options.UseSqlServer(connectionString,
                            m => m.MigrationsAssembly(typeof(IdentityContextSocial).Assembly.FullName));
                    },
                    contextLifetime: ServiceLifetime.Scoped,
                    optionsLifetime: ServiceLifetime.Scoped
                    );
            }
        }
    }
}