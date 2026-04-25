using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IUserProfileRepositoryAsync : IGenericRepositoryAsync<UserProfile>
    {
        
        Task<UserProfile> GetUserByOtpAsync(int otp);
        Task<UserProfile> GetUserByEmailAsync(string email);
        Task<UserProfile> GetUserByIdAsync(int userId);
        Task<UserProfile> GetUserProfileByIdAsync(int userId);
       // Task<List<int>> GetUserIdsByEmail(List<string> emails);
        List<int> GetUserIdsByEmail(List<string> emails);
        IQueryable<UserProfile> GetUserProfilesByIds(List<int> ids);
        IQueryable<UserProfile> GetUserProfilesByIds(IQueryable<int> ids);
        Task<UserProfile> GetUserProfilesByIdLite(int id);
        IQueryable<UserProfile> GetAllUsers();
    }
}
