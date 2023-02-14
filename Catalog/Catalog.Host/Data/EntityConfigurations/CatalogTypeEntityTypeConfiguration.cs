using Catalog.Host.Data.Entities;

namespace Catalog.Host.Data.EntityConfigurations
{
    public class CatalogTypeEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogType>
    {
        public void Configure(EntityTypeBuilder<CatalogType> builder)
        {
            builder.ToTable("CatalogType");

            builder.HasKey(h => h.Id);

            builder.Property(p => p.Id)
                .UseHiLo("catalog_type_hilo")
                .IsRequired();

            builder.Property(p => p.Type)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
