using Microsoft.EntityFrameworkCore;
using EasyTalk.Shared.Models;

namespace EasyTalk.Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions <AppDbContext> options) 
            : base(options)
        {   


        }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // pastikan relasi 1 Trainer : banyak Course
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Trainer)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
        