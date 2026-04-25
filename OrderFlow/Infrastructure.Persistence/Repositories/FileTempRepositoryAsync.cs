using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Infrastructure.Persistence.Repositories
{
    public class FileTempRepositoryAsync : GenericRepositoryAsync<FileTemp>, IFileTempRepositoryAsync
    {
        private readonly DbSet<FileTemp> _fileTemps;

        public FileTempRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _fileTemps = dbContext.Set<FileTemp>();
        }


        public IQueryable<FileTemp> GetFileTemps()
        {
            throw new NotImplementedException();
        }

        public FileTemp GetFileTempByUniqueName(string fileUniqueName)
        {
            var file = _fileTemps.Where(x => x.FileUniqueName.ToLower() == fileUniqueName.ToLower()).FirstOrDefault();

            return file;
        }

        public IQueryable<FileTemp> GetFileTempByMenuItemId(int menuItemId)
        {
           return _fileTemps.Where(x => x.MenuItemId == menuItemId);
        }

    }
}
