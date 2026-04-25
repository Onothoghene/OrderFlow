using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IAddressRepositoryAsync : IGenericRepositoryAsync<Address>
    {
        
        Task<Address> GetDefaultUserAddress(int userId);
        //Task<List<Address>> GetUserAddresses(int userId);
        public IQueryable<Address> GetUserAddresses(int userId);
    }
}
