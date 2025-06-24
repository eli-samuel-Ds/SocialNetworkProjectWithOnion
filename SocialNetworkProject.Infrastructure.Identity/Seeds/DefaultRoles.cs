using Microsoft.AspNetCore.Identity;
using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Infrastructure.Identity.Seeds
{
    public class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Administrador.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.UserNormal.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdministrador.ToString()));
        }
    }
}
