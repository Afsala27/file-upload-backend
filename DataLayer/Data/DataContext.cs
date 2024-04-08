using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Data
{
    public class DataContext : IdentityDbContext<AppUser>
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
                .HasOne(img => img.AppUsers)
                .WithMany(user => user.ImgDataList)
                .HasForeignKey(img => img.Id)
                .OnDelete(DeleteBehavior.Cascade);
                // Choose the appropriate delete behavior

            base.OnModelCreating(modelBuilder);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName ="ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName ="USER"
                },

            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}