using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Application.Enums;

namespace Infrastructure.Persistence.Seeds
{
    public static class DefaultMenuItems
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (!await dbContext.MenuItems.AnyAsync())
            {
                var menuItems = new[]
                {
                    new MenuItem { Name = "Cheeseburger", Price = 5.99m, Description = "Delicious beef burger", CategoryId = (int)FoodCategoryEnum.Breakfast, StockQuantity = 4 },
                    new MenuItem { Name = "Veggie Wrap", Price = 4.99m, Description = "Healthy veggie wrap", CategoryId = (int)FoodCategoryEnum.Lunch, StockQuantity = 2 },
                    new MenuItem { Name = "Chicken Wings", Price = 6.99m, Description = "Spicy chicken wings", CategoryId = (int)FoodCategoryEnum.Dinner, StockQuantity = 6 },
                    new MenuItem { Name = "Margherita Pizza", Price = 7.99m, Description = "Classic pizza", CategoryId = (int)FoodCategoryEnum.Breakfast, StockQuantity = 8 },
                    new MenuItem { Name = "Chocolate Cake", Price = 3.99m, Description = "Rich chocolate cake", CategoryId = (int)FoodCategoryEnum.Snacks, StockQuantity = 13 },
                    new MenuItem { Name = "Caesar Salad", Price = 4.99m, Description = "Fresh salad with Caesar dressing", CategoryId = (int)FoodCategoryEnum.Lunch, StockQuantity = 40 },
                    new MenuItem { Name = "BBQ Ribs", Price = 12.99m, Description = "Tender BBQ ribs", CategoryId = (int)FoodCategoryEnum.Dinner, StockQuantity = 0 },
                    new MenuItem { Name = "Fish Tacos", Price = 5.99m, Description = "Crispy fish tacos", CategoryId = (int)FoodCategoryEnum.Breakfast, StockQuantity = 7 },
                    new MenuItem { Name = "Spaghetti Bolognese", Price = 6.99m, Description = "Pasta with meat sauce", CategoryId = (int)FoodCategoryEnum.Lunch, StockQuantity = 2 },
                    new MenuItem { Name = "Ice Cream Sundae", Price = 3.49m, Description = "Vanilla ice cream with toppings", CategoryId = (int)FoodCategoryEnum.Snacks, StockQuantity = 1 },
                    new MenuItem { Name = "Pancakes", Price = 5.49m, Description = "Fluffy pancakes with syrup", CategoryId = (int)FoodCategoryEnum.Breakfast, StockQuantity = 3 },
                    new MenuItem { Name = "Grilled Chicken", Price = 7.99m, Description = "Grilled chicken with herbs", CategoryId = (int)FoodCategoryEnum.Lunch, StockQuantity = 4 },
                    new MenuItem { Name = "Lobster Roll", Price = 9.99m, Description = "Fresh lobster roll", CategoryId = (int)FoodCategoryEnum.Dinner, StockQuantity = 3 },
                    new MenuItem { Name = "Fruit Smoothie", Price = 4.49m, Description = "Mixed fruit smoothie", CategoryId = (int)FoodCategoryEnum.Lunch, StockQuantity = 2 }
                };

                await dbContext.MenuItems.AddRangeAsync(menuItems);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
