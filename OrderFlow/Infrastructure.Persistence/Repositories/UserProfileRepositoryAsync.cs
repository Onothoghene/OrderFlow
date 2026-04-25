using Application.Enums;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class UserProfileRepositoryAsync : GenericRepositoryAsync<UserProfile>, IUserProfileRepositoryAsync
    {
        private readonly DbSet<UserProfile> _userProfile;

        public UserProfileRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _userProfile = dbContext.Set<UserProfile>();
        }

        public Task<UserProfile> GetUserByOtpAsync(int otp)
        {
            return _userProfile.Where(x => x.VerificationCode == otp).FirstOrDefaultAsync();
        }

        public Task<UserProfile> GetUserByEmailAsync(string email)
        {
            return _userProfile.Where(x => x.Email == email).FirstOrDefaultAsync();
        }

        public Task<UserProfile> GetUserByIdAsync(int userId)
        {
            return _userProfile.Where(x => x.Id == userId)
                .FirstOrDefaultAsync();
        }

        public Task<UserProfile> GetUserProfileByIdAsync(int userId)
        {
            return _userProfile.Where(x => x.Id == userId)

                .FirstOrDefaultAsync();
        }

        public List<int> GetUserIdsByEmail(List<string> emails)
        {
            var users = _userProfile.Where(x => emails.Contains(x.Email)).ToList();
            var userIds = users.Select(y => y.Id).ToList();
            return userIds;

        }

        public IQueryable<UserProfile> GetUserProfilesByIds(List<int> ids)
        {
            var users = _userProfile.Where(x => ids.Contains(x.Id));

            return users;
        }

        public IQueryable<UserProfile> GetUserProfilesByIds(IQueryable<int> ids)
        {
            var users = _userProfile.Where(x => ids.Contains(x.Id));

            return users;
        }

        public Task<UserProfile> GetUserProfilesByIdLite(int id)
        {
            var user = _userProfile.Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return user;
        }

        public IQueryable<UserProfile> GetUsersProjectSelections(List<int> studentIds)
        {
            return _userProfile.Where(x => x.IsDeleted == false && studentIds.Contains(x.Id));
                //.Include(r => r.ProjectSelection)
                //.Include(r => r.StudentSelection);
        }

        public IQueryable<UserProfile> GetAllUsers()
        {
            return _userProfile.AsQueryable();
        }

        public List<int> GetStudentsByEmails(List<string> emails)
        {
            var userIds = _userProfile.Where(x => emails.Contains(x.Email)).Select(y => y.Id).ToList();

            return userIds;
        }

        public List<int> GetSupervisorsByEmails(List<string> emails)
        {
            var userIds = _userProfile.Where(x => emails.Contains(x.Email)).Select(y => y.Id).ToList();

            return userIds;
        }
    }
}
