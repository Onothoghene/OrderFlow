using Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ICommentRepositoryAsync : IGenericRepositoryAsync<Comments>
    {
        IQueryable<Comments> GetFoodComments(int foodId);
        Task<Comments> GetCommentById(int id);
        IQueryable<Comments> GetUserComments(int userId);
        IQueryable<Comments> GetUserFoodComments(int userId, int foodId);
    }
}
