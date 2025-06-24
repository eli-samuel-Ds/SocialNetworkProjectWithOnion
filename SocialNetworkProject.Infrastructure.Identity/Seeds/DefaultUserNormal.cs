using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetworkProject.Core.Domain.Common.Enums;
using SocialNetworkProject.Infrastructure.Identity.Entities;

namespace SocialNetworkProject.Infrastructure.Identity.Seeds
{
    public class DefaultUserNormal
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager)
        {
            AppUser user = new()
            {
                FirstName = "Juan",
                LastName = "Perez",
                Email = "investor@email.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                UserName = "basic"
            };

            if (await userManager.Users.AllAsync(u => u.Id != user.Id))
            {
                var entityUser = await userManager.FindByEmailAsync(user.Email);
                if (entityUser == null)
                {
                    await userManager.CreateAsync(user, "123Pa$$word!");
                    await userManager.AddToRoleAsync(user, Roles.UserNormal.ToString());
                }
            }

        }
    }
}
}
