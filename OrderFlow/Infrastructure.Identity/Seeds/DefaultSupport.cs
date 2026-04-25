using Application.Enums;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Seeds
{
    public static class DefaultSupport
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminUsers = new[]
            {
                new ApplicationUser
                {
                    UserName = "superadmin1@gmail.com",
                    Email = "superadmin1@gmail.com",
                    FirstName = "Mukesh",
                    LastName = "Murugan",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                },
                new ApplicationUser
                {
                    UserName = "superadmin2@gmail.com",
                    Email = "superadmin2@gmail.com",
                    FirstName = "Jane",
                    LastName = "Doe",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                }
            };

            foreach (var admin in adminUsers)
            {
                if (!await userManager.Users.AnyAsync(u => u.Email == admin.Email))
                {
                    var result = await userManager.CreateAsync(admin, "Admin@123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
                    }
                }
            }
        }
    }
}
