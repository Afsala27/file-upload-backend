using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.DTOs.File;
using DataLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface IImageRepository
    {
        Task<ImgData?> GetByIdAsync(int id);
        //Task<IEnumerable<UserData>> GetAllAsync();
        Task AddAsync(ImgData images);
        Task UpdateAsync(ImgData images);
        Task DeleteAsync(ImgData images);
        Task UpdateCommentAsync(int imgDataId, string? newComment);
    }
}
