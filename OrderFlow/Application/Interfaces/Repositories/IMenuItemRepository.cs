using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IMenuItemRepositoryAsync : IGenericRepositoryAsync<MenuItem>
    {
        Task<MenuItem> GetMenuItemById(int id);
        //IQueryable<MenuItem> GetAllMenuItems();

        Task<List<MenuItem>> GetAllMenuItems();
        Task<List<MenuItem>> GetByIdsAsync(List<int> ids);
        Task<List<MenuItem>> GetByIdsWithTrackingAsync(List<int> ids);
    }
}
