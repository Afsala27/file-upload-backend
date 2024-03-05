using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using DataLayer.Data;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly DataContext  _dbContext;

        public ImageRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ImgData?> GetByIdAsync(int id)
        {
            return await _dbContext.ImgDatas.FindAsync(id);
        }

        // public async Task<IEnumerable<User>> GetAllAsync()
        // {
        //     return await _dbContext.Users.ToListAsync();
        // }

        public async Task AddAsync(ImgData images)
        {
            _dbContext.ImgDatas.Add(images);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(ImgData images)
        {
            _dbContext.Entry(images).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }


        public async Task UpdateCommentAsync(int imgDataId, string? newComment)
        {
            var imgData = await _dbContext.ImgDatas.FindAsync(imgDataId);

            if (imgData != null)
            {
                imgData.Comment = newComment;
                await _dbContext.SaveChangesAsync();
            }
        }


        public async Task DeleteAsync(ImgData images)
        {
            var det = await _dbContext.ImgDatas.FindAsync(images.ImgDataId);
            if (det != null)
            {
                _dbContext.ImgDatas.Remove(det); // check this user thing latter
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
