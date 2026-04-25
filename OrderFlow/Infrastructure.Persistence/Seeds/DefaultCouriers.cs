using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.Seeds
{
    public static class DefaultCouriers
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (!await dbContext.Couriers.AnyAsync())
            {
                var couriers = new[]
                {
                    new Courier { Name = "John Rider", RestaurantId = 1 },
                    new Courier { Name = "Alex Swift", RestaurantId = 2 },
                    new Courier { Name = "Chris Express", RestaurantId = 3 },
                    new Courier { Name = "Sarah Fast", RestaurantId = 4 },
                    new Courier { Name = "Mike Quick", RestaurantId = 5 }
                };

                await dbContext.Couriers.AddRangeAsync(couriers);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
