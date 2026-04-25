using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Seeds
{
    public static class DefaultUserProfile
    {
        public static async Task SeedAsync(ApplicationDbContext appDbContext)
        {
            if (!await appDbContext.UserProfile.AnyAsync())
            {
                //logger.LogInformation("Seeding User Profile data...");

                var profiles = new[]
                {
                    new UserProfile
                    {
                        //UserId = users.FirstOrDefault(u => u.Email == "basicuser1@gmail.com")?.Id,
                        FirstName = "John",
                        LastName = "Doe",
                        Email = "basicuser1@gmail.com",
                        PhoneNumber = "1234567890",
                        Created = DateTime.UtcNow,
                        VerificationCode = new Random().Next(100000, 999999)
                    },
                    new UserProfile
                    {
                        //UserId = users.FirstOrDefault(u => u.Email == "basicuser2@gmail.com")?.Id,
                        FirstName = "Jane",
                        LastName = "Smith",
                        Email = "basicuser2@gmail.com",
                        PhoneNumber = "0987654321",
                        Created = DateTime.UtcNow,
                        VerificationCode = new Random().Next(1000, 9999)
                    },
                    new UserProfile
                    {
                        //UserId = users.FirstOrDefault(u => u.Email == "basicuser3@gmail.com")?.Id,
                        FirstName = "Mike",
                        LastName = "Johnson",
                        Email = "basicuser3@gmail.com",
                        PhoneNumber = "5551234567",
                        Created = DateTime.UtcNow,
                        VerificationCode = new Random().Next(1000, 9999)
                    }, 
                    //For Admin Users
                     new UserProfile
                    {
                        FirstName = "Mukesh",
                        LastName = "Murugan",
                        Email = "superadmin1@gmail.com",
                        PhoneNumber = "1234567890",
                        Created = DateTime.UtcNow,
                        VerificationCode = new Random().Next(1000, 9999)
                    },
                    new UserProfile
                    {
                        FirstName = "Jane",
                        LastName = "Doe",
                        Email = "superadmin2@gmail.com",
                        PhoneNumber = "0987654321",
                        Created = DateTime.UtcNow,
                        VerificationCode = new Random().Next(1000, 9999)
                    }
                };

                await appDbContext.UserProfile.AddRangeAsync(profiles);
                await appDbContext.SaveChangesAsync();

                //logger.LogInformation("UserProfile(s) seeded Successfully...");
            }
            //else
            //{
            //    //logger.LogInformation("UserProfiles Already exist(s) so skipped...");
            //}
        }
    }
}