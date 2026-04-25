using Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IMenuItemRatingRepositoryAsync : IGenericRepositoryAsync<MenuItemRating>
    {
        IQueryable<MenuItemRating> GetItemRating(int itemId);
        IQueryable<MenuItemRating> GetUserRatings(int userId);
        Task<double> GetAverageRatingAsync(int menuItemId);
    }
}
