using CatalogWebAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogWebAPI.Data.EntityConfigurations
{
    public class CatalogGenreEntityConfiguration : IEntityTypeConfiguration<CatalogGenreEntity>
    {
        public void Configure(EntityTypeBuilder<CatalogGenreEntity> builder)
        {
            builder.ToTable("CatalogGenre");

            builder.HasKey(cg => cg.Id);

            builder.Property(cg => cg.Id)
                .UseHiLo("catalog_genre_hilo")
                .IsRequired();

            builder.Property(cg => cg.Genre).IsRequired().HasMaxLength(100);
        }
    }
}
