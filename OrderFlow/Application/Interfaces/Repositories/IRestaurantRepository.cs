using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IRestaurantRepositoryAsync : IGenericRepositoryAsync<Restaurant>
    {
        Task<Restaurant> GetRestaurantbyId(int id);
        //IQueryable<Restaurant> GetAllRestaurants();
        Task<List<Restaurant>> GetAllRestaurants();
    }
}
