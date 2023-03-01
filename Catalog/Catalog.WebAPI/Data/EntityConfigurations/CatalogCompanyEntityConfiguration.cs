using CatalogWebAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogWebAPI.Data.EntityConfigurations
{
    public class CatalogCompanyEntityConfiguration : IEntityTypeConfiguration<CatalogCompanyEntity>
    {
        public void Configure(EntityTypeBuilder<CatalogCompanyEntity> builder)
        {
            builder.ToTable("CatalogCompany");

            builder.HasKey(cc => cc.Id);

            builder.Property(cc => cc.Id)
                .UseHiLo("catalog_company_hilo")
                .IsRequired();

            builder.Property(cc => cc.Company).IsRequired().HasMaxLength(75);
        }
    }
}
