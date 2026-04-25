using Domain.Entities;
using System.Linq;

namespace Application.Interfaces.Repositories
{
    public interface IFileTempRepositoryAsync : IGenericRepositoryAsync<FileTemp>
    {
        IQueryable<FileTemp> GetFileTemps();
        FileTemp GetFileTempByUniqueName(string fileUniqueName);
        IQueryable<FileTemp> GetFileTempByMenuItemId(int menuItemId);
    }
}
