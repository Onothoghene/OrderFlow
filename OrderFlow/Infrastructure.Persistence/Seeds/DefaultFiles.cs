using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.Seeds
{
    public static class DefaultFiles
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (!await dbContext.FileTemp.AnyAsync())
            {
                var fileTemps = new[]
                {
                    //new MenuItem { Name = "Cheeseburger"},
                    new FileTemp { FileName = "Cheeseburger",
                                   FileExt = "",
                                   FileUniqueName = "",
                                   ImageURL = "https://i.pinimg.com/736x/3b/6c/c6/3b6cc651e4cd1cbc7fa7d559a5b81810.jpg",
                                   MenuItemId = 1 },
                    //new MenuItem { Name = "Veggie Wrap"},
                    new FileTemp { FileName = "Veggie Wrap",
                                   FileExt = "",
                                   FileUniqueName = "",
                                   ImageURL = "https://i.pinimg.com/736x/aa/1e/b6/aa1eb63a90ea13dc84e9c9c7276a6522.jpg",
                                   MenuItemId = 2 },
                    //new MenuItem { Name = "Chicken Wings"},
                    new FileTemp { FileName = "Chicken Wings",
                                   FileExt = "",
                                   FileUniqueName = "",
                                   ImageURL = "https://i.pinimg.com/736x/e5/e8/27/e5e8277fb04cf3c77919139170828bbf.jpg",
                                   MenuItemId = 3 },
                    //new MenuItem { Name = "Margherita Pizza"},
                    new FileTemp { FileName = "Margherita Pizza",
                                   FileExt = "",
                                   FileUniqueName = "",
                                   ImageURL = "https://i.pinimg.com/736x/35/50/30/35503019b6a63937f04c0b689ec90b34.jpg",
                                   MenuItemId = 4 },
                    //new MenuItem { Name = "Chocolate Cake"},
                    new FileTemp { FileName = "Chocolate Cake",
                                   FileExt = "",
                                   FileUniqueName = "",
                                   ImageURL = "https://i.pinimg.com/736x/03/d4/b1/03d4b1563efdbd8325785d856f812bb9.jpg",
                                   MenuItemId = 5 },
                    //new MenuItem { Name = "Caesar Salad"},
                    new FileTemp { FileName = "Caesar Salad",
                                   FileExt = "",
                                   FileUniqueName = "",
                                   ImageURL = "https://i.pinimg.com/736x/b1/2c/4d/b12c4d8a15f84eaabe3e83f2d21022d5.jpg",
                                   MenuItemId = 6 },
                    //new MenuItem { Name = "BBQ Ribs"},
                    new FileTemp { FileName = "BBQ Ribs",
                                   FileExt = "",
                                   FileUniqueName = "",
                                   ImageURL = "https://i.pinimg.com/736x/f9/b6/bd/f9b6bde0589cfb671c65e08820b7a166.jpg",
                                   MenuItemId = 7 },
                    //new MenuItem { Name = "Fish Tacos" },
                    new FileTemp { FileName = "Fish Tacos",
                                   FileExt = "",
                                   FileUniqueName = "",
                                   ImageURL = "https://i.pinimg.com/736x/07/74/b1/0774b1109877cf9bb54609230cc817d0.jpg",
                                   MenuItemId = 8 },
                    //new MenuItem { Name = "Spaghetti Bolognese"},
                    new FileTemp { FileName = "Spaghetti Bolognese",
                                   FileExt = "",
                                   FileUniqueName = "",
                                   ImageURL = "https://i.pinimg.com/736x/94/ed/51/94ed516e1d1ed82e6a1da3c86ea0877a.jpg",
                                   MenuItemId = 9 },
                    //new MenuItem { Name = "Ice Cream Sundae"},
                    new FileTemp { FileName = "Ice Cream Sundae",
                                   FileExt = "",
                                   FileUniqueName = "",
                                   ImageURL = "https://i.pinimg.com/736x/53/3f/88/533f88257f876735c8734a6921466b94.jpg",
                                   MenuItemId = 10 },
                    //new MenuItem { Name = "Pancakes"},
                    new FileTemp { FileName = "Pancakes",
                                   FileExt = "",
                                   FileUniqueName = "",
                                   ImageURL = "https://i.pinimg.com/736x/d0/bd/dc/d0bddce4efb65033c4422ac3e92c1edf.jpg",
                                   MenuItemId = 11 },
                    //new MenuItem { Name = "Grilled Chicken"},
                    new FileTemp { FileName = "Grilled Chicken",
                                   FileExt = "",
                                   FileUniqueName = "",
                                   ImageURL = "https://i.pinimg.com/736x/63/37/a8/6337a8d40d36f6faddb1c553a92d9ede.jpg",
                                   MenuItemId = 12 },
                    //new MenuItem { Name = "Lobster Roll"},
                    new FileTemp { FileName = "Lobster Roll",
                                   FileExt = "",
                                   FileUniqueName = "",
                                   ImageURL = "https://i.pinimg.com/736x/a6/5d/76/a65d7645837f05ee8414535ad2046857.jpg",
                                   MenuItemId = 13 },
                    //new MenuItem { Name = "Fruit Smoothie"}
                    new FileTemp { FileName = "Fruit Smoothie",
                                   FileExt = "",
                                   FileUniqueName = "",
                                   ImageURL = "https://i.pinimg.com/736x/34/09/9d/34099d3b61e04f41b9aac36b873dd0d4.jpg",
                                   MenuItemId = 14 },
                };

                await dbContext.FileTemp.AddRangeAsync(fileTemps);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
