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
    public class CourierRepositoryAsync : GenericRepositoryAsync<Courier>, ICourierRepositoryAsync
    {
        private readonly DbSet<Courier> _courier;

        public CourierRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _courier = dbContext.Set<Courier>();
        }

        public IQueryable<Courier> GetAllCouriers()
        {
            return _courier.Include(r => r.Restaurant).AsQueryable();
        }

        public Task<Courier> GetCourierById(int id)
        {
            return _courier.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Courier> GetRestaurantCourier(int restaurantId)
        {
            return _courier.FirstOrDefaultAsync(x => x.RestaurantId == restaurantId);
        }
    }
}
