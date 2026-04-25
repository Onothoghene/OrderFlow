using Application.Enums;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class OrderRepositoryAsync : GenericRepositoryAsync<Orders>, IOrderRepositoryAsync
    {
        private readonly DbSet<Orders> _orders;

        public OrderRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _orders = dbContext.Set<Orders>();
        }

        public IQueryable<Orders> GetAllOrders()
        {
            return _orders.Include(r => r.OrderItems)
                            .AsQueryable();
        }

        public async Task<Orders> GetOrderById(int id)
        {
            var res =  await _orders.Include(o => o.OrderItems)
                                    .ThenInclude(r => r.Food)
                                //.Include(o => o.Restaurant)
                                .Include(o => o.CreatedByNavigation)
                                .Include(o => o.Payments)
                                //.Include(o => o.Address)
                                .Where(u => u.Id == id)
                                 .FirstOrDefaultAsync();
            return res;
        }

        public IQueryable<Orders> GetUserOrders(int userId)
        {
            return _orders.Where(i => i.CreatedBy == userId).Include(o => o.OrderItems)
                                .Include(o => o.Restaurant)
                                .ThenInclude(r => r.Courier)
                                .Include(o => o.CreatedByNavigation)
                                .Include(o => o.Payments)
                                .Include(o => o.AddressId)
                                .AsQueryable(); ;
        }

        public async Task<List<Orders>> GetOrdersByRestaurantIdAsync(int restaurantId)
        {
            return await _orders.Where(o => o.RestaurantId == restaurantId)
                                   .Include(o => o.OrderItems)
                                   .ToListAsync();
        }

        public async Task<List<Orders>> GetPendingOrdersAsync()
        {
            return await _orders.Where(o => o.Status == (int)OrderEnum.Pending &&  // Filter by Enum
                            EF.Functions.DateDiffMinute(o.Created, DateTime.UtcNow) >= 15)
                .ToListAsync();
        }

        public async Task<Orders> GetOrderSummary(int orderId)
        {
            var res = await _orders.Include(o => o.OrderItems)
                                .Include(o => o.Restaurant)
                                //.Include(o => o.CreatedByNavigation)
                                .Include(o => o.Payments)
                                //.Include(o => o.Address)
                                .Where(r => r.Id == orderId)
                                .FirstOrDefaultAsync();
            return res;
        }
    }
}
