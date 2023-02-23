using CatalogWebAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogWebAPI.Data.EntityConfigurations
{
    public class CatalogItemEntityConfiguration : IEntityTypeConfiguration<CatalogItemEntity>
    {
        public void Configure(EntityTypeBuilder<CatalogItemEntity> builder)
        {
            builder.ToTable("Catalog");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .UseHiLo("catalog_item_hilo")
                .IsRequired();

            builder.Property(ci => ci.Name).IsRequired().HasMaxLength(50);
            builder.Property(ci => ci.Price).IsRequired();
            builder.Property(ci => ci.Year).IsRequired();

            builder.HasOne(ci => ci.CatalogCompany)
                .WithMany()
                .HasForeignKey(ci => ci.CatalogCompanyId);

            builder.HasOne(ci => ci.CatalogGenre)
                .WithMany()
                .HasForeignKey(ci => ci.CatalogGenreId);
        }
    }
}
