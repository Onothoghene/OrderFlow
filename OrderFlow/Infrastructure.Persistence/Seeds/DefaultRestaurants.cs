using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.Seeds
{
    public static class DefaultRestaurants
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (!await dbContext.Restaurants.AnyAsync())
            {
                var restaurants = new[]
                {
                    new Restaurant { Name = "Tasty Bites", Location = "12 Adeola Odeku St, Victoria Island, Lagos", IsAvailable = true },
                    new Restaurant { Name = "Ocean Delights", Location = "3A Akin Adesola St, Victoria Island, Lagos", IsAvailable = true },
                    new Restaurant { Name = "Burger Hub", Location = "15 Isaac John St, Ikeja GRA, Lagos", IsAvailable = true },
                    new Restaurant { Name = "Spicy Treats", Location = "42 Admiralty Way, Lekki Phase 1, Lagos", IsAvailable = true },
                    new Restaurant { Name = "Healthy Greens", Location = "18B Agungi Rd, Lekki, Lagos", IsAvailable = true }
                };

                await dbContext.Restaurants.AddRangeAsync(restaurants);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
