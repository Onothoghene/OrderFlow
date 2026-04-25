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
    public class RestaurantRepositoryAsync : GenericRepositoryAsync<Restaurant>, IRestaurantRepositoryAsync
    {
        private readonly DbSet<Restaurant> _restaurant;

        public RestaurantRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _restaurant = dbContext.Set<Restaurant>();
        }

        //public async Task<List<Restaurant>> GetAllRestaurants()
        //{
        //    return await _restaurant.Include(r => r.Courier).ToListAsync();
        //}

        public async Task<List<Restaurant>> GetAllRestaurants()
        {
            var restaurants = await _restaurant.ToListAsync();
            if (restaurants == null || !restaurants.Any())
            {
                // Add logging here to check if data is being returned
                Console.WriteLine("No restaurants found in the database.");
            }
            return restaurants;
        }


        public Task<Restaurant> GetRestaurantbyId(int id)
        {
            return _restaurant.Where(x => x.Id == id).Include(r => r.Courier).FirstOrDefaultAsync();
        }
    }
}
