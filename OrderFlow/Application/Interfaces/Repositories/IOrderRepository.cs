using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IOrderRepositoryAsync : IGenericRepositoryAsync<Orders>
    {
        Task<Orders> GetOrderById(int id);
        IQueryable<Orders> GetUserOrders(int userId);
        IQueryable<Orders> GetAllOrders();
        Task<List<Orders>> GetOrdersByRestaurantIdAsync(int restaurantId);
        Task<List<Orders>> GetPendingOrdersAsync();
        Task<Orders> GetOrderSummary(int orderId);
    }
}
