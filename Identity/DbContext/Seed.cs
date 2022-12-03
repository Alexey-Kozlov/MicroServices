using Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Identity.DbContext
{
    public class Seed
    {
        public static async Task SeedUser(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<ApplicationUser>
                {
                    new ApplicationUser
                    {
                        DisplayName = "Админ",
                        UserName = "admin"
                    },
                    new ApplicationUser
                    {
                        DisplayName = "Тест",
                        UserName = "user"
                    }
                };

                int i = 0;
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "admin");
                    if (i == 0)
                    {
                        var adminRole = await roleManager.FindByNameAsync("Admin");
                        if (adminRole != null)
                        {
                            await userManager.AddToRoleAsync(user, adminRole.Name);
                        }
                    }
                    else
                    {
                        var userRole = await roleManager.FindByNameAsync("User");
                        if (userRole != null)
                        {
                            await userManager.AddToRoleAsync(user, userRole.Name);
                        }
                    }
                    i++;
                }
            }
        }

        public static async Task SeedRole(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                var roles = new List<IdentityRole>
                {
                    new IdentityRole
                    {
                        Name = "Admin"
                    },
                    new IdentityRole
                    {
                        Name = "User"
                    }
                };
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }
    }
}
