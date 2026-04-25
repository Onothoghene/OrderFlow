using Application.Interfaces.Repositories;
using Domain.Entities;
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
    public class CartItemRepositoryAsync : GenericRepositoryAsync<CartItems>, ICartItemRepository
    {
        private readonly DbSet<CartItems> _cartItems;

        public CartItemRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _cartItems = dbContext.Set<CartItems>();
        }

        public async Task<List<CartItems>> GetCartByUserIdAsync(int userId)
        {
            return await _cartItems.Where(x => x.CreatedBy == userId && !x.Ordered)
                                   .Include(r => r.Food)
                                   .ThenInclude(o => o.Images)
                                   .ToListAsync();
        }

        public async Task<CartItems> GetUserMenuCartAsync(int userId, int menuItemId)
        {
            return await _cartItems.Where(x => x.CreatedBy == userId && x.FoodId == menuItemId
                                                    && x.Ordered == false)
                                                    .FirstOrDefaultAsync();
        }

        public async Task<List<CartItems>> GetCartItemsAsync(int userId, List<int>cartItemsId)
        {
            return await _cartItems.Where(x => x.CreatedBy == userId && cartItemsId.Contains(x.FoodId))
                                                .ToListAsync();
        }
    }
}
