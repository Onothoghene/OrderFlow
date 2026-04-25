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
    public class AddressRepositoryAsync : GenericRepositoryAsync<Address>, IAddressRepositoryAsync
    {
        private readonly DbSet<Address> _address;

        public AddressRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _address = dbContext.Set<Address>();
        }

        public Task<Address> GetDefaultUserAddress(int userId)
        {
            return  _address.Where(x => x.IsDefault && x.CreatedBy ==  userId && !x.IsDeleted).FirstOrDefaultAsync();
        }

        public IQueryable<Address> GetUserAddresses(int userId)
        {
            return  _address.Where(x => x.CreatedBy == userId).AsQueryable();
        }
    }
}
