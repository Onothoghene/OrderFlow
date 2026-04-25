using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ICartItemRepository : IGenericRepositoryAsync<CartItems>
    {
        Task<List<CartItems>> GetCartByUserIdAsync(int userId);
        Task<CartItems> GetUserMenuCartAsync(int userId, int menuItemId);
        Task<List<CartItems>> GetCartItemsAsync(int userId, List<int> cartItemsId);
    }
}
