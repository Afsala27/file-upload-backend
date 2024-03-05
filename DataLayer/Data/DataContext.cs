using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<UserData> UserDatas {get; set;}

        public DbSet<ImgData> ImgDatas {get; set;}

        //new connection between the tables
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImgData>()
                .HasOne(img => img.UserData)
                .WithMany(user => user.ImgDataList)
                .HasForeignKey(img => img.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                // Choose the appropriate delete behavior

            base.OnModelCreating(modelBuilder);
        }
    }
}