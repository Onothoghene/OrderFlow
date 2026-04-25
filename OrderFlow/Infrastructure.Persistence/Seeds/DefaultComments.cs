using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.Seeds
{
    public static class DefaultComments
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (!await dbContext.Comments.AnyAsync())
            {
                var comments = new[]
                {
                    new Comments { FoodId = 1, CommentText = "Tastes amazing!", Rating = 4.5 },
                    new Comments { FoodId = 2, CommentText = "Really fresh and healthy", Rating = 4.8 },
                    new Comments { FoodId = 3, CommentText = "Too spicy for me", Rating = 3.0 },
                    new Comments { FoodId = 4, CommentText = "Best pizza in town!", Rating = 5.0 },
                    new Comments { FoodId = 5, CommentText = "Super chocolatey!", Rating = 4.7 },
                    new Comments { FoodId = 6, CommentText = "Loved the salad", Rating = 4.6 },
                    new Comments { FoodId = 7, CommentText = "BBQ was perfect", Rating = 4.9 },
                    new Comments { FoodId = 8, CommentText = "Tacos could use more spice", Rating = 3.9 },
                    new Comments { FoodId = 9, CommentText = "Pasta was great!", Rating = 4.4 },
                    new Comments { FoodId = 10, CommentText = "Nice dessert!", Rating = 4.2 },
                    new Comments { FoodId = 11, CommentText = "Pancakes were fluffy", Rating = 4.8 },
                    new Comments { FoodId = 12, CommentText = "Chicken was a bit dry", Rating = 3.5 },
                    new Comments { FoodId = 13, CommentText = "Lobster roll was fresh", Rating = 4.6 },
                    new Comments { FoodId = 14, CommentText = "Smoothie was refreshing", Rating = 4.9 },
                    new Comments { FoodId = 15, CommentText = "Would order again!", Rating = 4.5 }
                };

                await dbContext.Comments.AddRangeAsync(comments);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
