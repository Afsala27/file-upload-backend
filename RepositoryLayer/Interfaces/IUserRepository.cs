using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.DTOs.User;
using DataLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepository
    {
        Task<UserData?> GetByIdAsync(int id);
        //Task<IEnumerable<UserData>> GetAllAsync();
        //Task AddAsync(UserDto user);

        Task<UserData?> GetByEmailAsync(string? email);
        void GetByEmailAsync();
        //Task UpdateAsync(User user);
        // Task DeleteAsync(int id);
    }
}
