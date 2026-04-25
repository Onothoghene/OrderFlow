using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ICourierRepositoryAsync : IGenericRepositoryAsync<Courier>
    {
        Task<Courier> GetCourierById(int id);
        Task<Courier> GetRestaurantCourier(int restaurantId);
        IQueryable<Courier> GetAllCouriers();
    }
}
