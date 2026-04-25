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
    public class CommentRepositoryAsync : GenericRepositoryAsync<Comments>, ICommentRepositoryAsync
    {
        private readonly DbSet<Comments> _comments;

        public CommentRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _comments = dbContext.Set<Comments>();
        }

        public Task<Comments> GetCommentById(int id)
        {
            return _comments.FirstOrDefaultAsync(c => c.Id == id);
        }

        public IQueryable<Comments> GetFoodComments(int foodId)
        {
            return _comments.Where(x => x.FoodId == foodId);
        }

        public IQueryable<Comments> GetUserComments(int userId)
        {
            return _comments.Where(x => x.CreatedBy  == userId);
        }

        public IQueryable<Comments> GetUserFoodComments(int userId, int foodId)
        {
            return _comments.Where(x => x.CreatedBy == userId &&  x.FoodId == foodId);
        }
    }
}
