using SergeyPetrovKT_31_20.Database.Configurations;
using SergeyPetrovKT_31_20.Models;
using Microsoft.EntityFrameworkCore;

namespace SergeyPetrovKT_31_20.Database
{
    public class SerDbContext : DbContext

    {
        internal DbSet<Student> Students { get; set; }
        internal DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
        }
        public SerDbContext(DbContextOptions<SerDbContext> options) : base(options) { }
    }
}
