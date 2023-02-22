using CatalogWebAPI.Data.Entities;
using CatalogWebAPI.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CatalogCompanyEntity> CatalogCompanies { get; set; } = null!;
        public DbSet<CatalogGenreEntity> CatalogGenres { get; set; } = null!;
        public DbSet<CatalogItemEntity> CatalogItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CatalogCompanyEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CatalogGenreEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CatalogItemEntityConfiguration());
        }
    }
}
