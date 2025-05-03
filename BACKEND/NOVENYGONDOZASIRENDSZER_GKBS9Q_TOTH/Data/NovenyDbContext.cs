using Microsoft.EntityFrameworkCore;
using NOVENYGONDOZASIRENDSZER_GKBS9Q_TOTH.Models;

namespace NOVENYGONDOZASIRENDSZER_GKBS9Q_TOTH.Data
{
    public class NovenyDbContext : DbContext
    {
        public DbSet<Noveny> Novenyek { get; set; }
        public NovenyDbContext(DbContextOptions<NovenyDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NovenyDb;Integrated Security=True;MultipleActiveResultSets=True";
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Noveny>()
                .HasKey(n => n.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
