using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class MenuItemRepositoryAsync : GenericRepositoryAsync<MenuItem>, IMenuItemRepositoryAsync
    {
        private readonly DbSet<MenuItem> _menuItems;

        public MenuItemRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _menuItems = dbContext.Set<MenuItem>();
        }

        //public IQueryable<MenuItem> GetAllMenuItems()
        //{
        //    return _menuItems.Include(r => r.Images)
        //                     .Include(r => r.Comments)
        //                     .AsQueryable();
        //}

        public async Task<List<MenuItem>> GetAllMenuItems()
        {
            return await _menuItems
                         .Include(r => r.Comments)
                         .Include(r => r.Images)
                         .Include(r => r.CartItems)
                         .ThenInclude(x => x.CreatedByNavigation)
                         .ToListAsync();
        }

        public async Task<MenuItem> GetMenuItemById(int id)
        {
            return await _menuItems
                         .Include(r => r.Images)
                         .Include(r => r.Comments)
                         .Include(r => r.CartItems)
                         .ThenInclude(x => x.CreatedByNavigation)
                         .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<MenuItem>> GetByIdsAsync(List<int> ids)
        {
            return await _menuItems.Where(x => ids.Contains(x.Id))
                                   .AsNoTracking()
                                   .ToListAsync();
        }

        public async Task<List<MenuItem>> GetByIdsWithTrackingAsync(List<int> ids)
        {
            return await _menuItems.Where(x => ids.Contains(x.Id))
                                   .ToListAsync();
        }

    }
}
