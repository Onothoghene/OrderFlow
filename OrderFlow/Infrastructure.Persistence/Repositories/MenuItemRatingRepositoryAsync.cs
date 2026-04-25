using Application.Interfaces.Repositories;
using Domain.Entities;
using Hangfire.Dashboard;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class MenuItemRatingRepositoryAsync : GenericRepositoryAsync<MenuItemRating>, IMenuItemRatingRepositoryAsync
    {
        private readonly DbSet<MenuItemRating> _ratings;

        public MenuItemRatingRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _ratings = dbContext.Set<MenuItemRating>();
        }

        public IQueryable<MenuItemRating> GetItemRating(int itemId)
        {
            return _ratings.Where(x => x.MenuItemId == itemId).AsQueryable();
        }

        public IQueryable<MenuItemRating> GetUserRatings(int userId)
        {
            return _ratings.Where(x => x.CreatedBy == userId).AsQueryable();
        }

        public async Task<double> GetAverageRatingAsync(int menuItemId)
        {
            var ratings = await _ratings.Where(r => r.MenuItemId == menuItemId).ToListAsync();

            if (ratings.Count == 0) return 0;

            return ratings.Average(r => r.Rating);
        }
    }
}
