using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepository
    {
        Task<UserData?> GetByIdAsync(int id);
        //Task<IEnumerable<UserData>> GetAllAsync();
        Task AddAsync(UserData user);

        Task<UserData?> GetByEmailAsync(string? email);
        //Task UpdateAsync(User user);
       // Task DeleteAsync(int id);
    }
}
