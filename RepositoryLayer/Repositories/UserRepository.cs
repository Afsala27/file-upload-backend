using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using DataLayer.Data;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext  _dbContext;

        public UserRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserData?> GetByIdAsync(int id)
        {
            return await _dbContext.UserDatas.FindAsync(id);
        }

        // public async Task<IEnumerable<User>> GetAllAsync()
        // {
        //     return await _dbContext.Users.ToListAsync();
        // }

        public async Task AddAsync(UserData user)
        {
            _dbContext.UserDatas.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserData?> GetByEmailAsync(string? email)
        {
            return await _dbContext.UserDatas.FirstOrDefaultAsync(u => u.Email == email);
        }

        public void GetByEmailAsync()
        {
            throw new NotImplementedException();
        }
    }
}
