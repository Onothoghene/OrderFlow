using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Seeds
{
    public static class DefaultAdminUserProfile
    {
        public static async Task SeedAsync(ApplicationDbContext appDbContext)
        {
            if (!await appDbContext.UserProfile.AnyAsync())
            {
                var profiles = new[]
                {
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
            }
        }
    }
}
