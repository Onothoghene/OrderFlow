using Application.Enums;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Seeds
{
    public static class DefaultBasicUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var users = new[]
             {
                new ApplicationUser
                {
                    UserName = "basicuser1@gmail.com",
                    Email = "basicuser1@gmail.com",
                    FirstName = "John",
                    LastName = "Doe",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                },
                new ApplicationUser
                {
                    UserName = "basicuser2@gmail.com",
                    Email = "basicuser2@gmail.com",
                    FirstName = "Jane",
                    LastName = "Smith",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                },
                new ApplicationUser
                {
                    UserName = "basicuser3@gmail.com",
                    Email = "basicuser3@gmail.com",
                    FirstName = "Mike",
                    LastName = "Johnson",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                }
            };

            foreach (var defaultUser in users)
            {
                if (userManager.Users.All(u => u.Email != defaultUser.Email))
                {
                    var user = await userManager.FindByEmailAsync(defaultUser.Email);
                    if (user == null)
                    {
                        await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                        await userManager.AddToRoleAsync(defaultUser, Roles.User.ToString());
                    }
                }
            }
        }
    }
}
