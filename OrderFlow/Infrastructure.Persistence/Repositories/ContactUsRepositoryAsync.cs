using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public class ContactUsRepositoryAsync : GenericRepositoryAsync<ContactUs>, IContactUsRepositoryAsync
    {
        private readonly DbSet<ContactUs> _contactUs;

        public ContactUsRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _contactUs = dbContext.Set<ContactUs>();
        }

      
    }
}
