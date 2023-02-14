using Catalog.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Host.Data.EntityConfigurations
{
    public class CatalogItemEntityTypeConfiguration
        : IEntityTypeConfiguration<CatalogItem>
    {
        public void Configure(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.ToTable("Catalog");

            builder.Property(p => p.Id)
            .UseHiLo("catalog_hilo")
            .IsRequired();

            builder.Property(p => p.Title)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.Property(p => p.Price)
                .HasColumnType("money")
                .IsRequired(true);

            builder.Property(p => p.Weight)
                .IsRequired(true);

            builder.Property(p => p.PictureFileName)
                .IsRequired(false);

            builder.HasOne(h => h.CatalogType)
                .WithMany()
                .HasForeignKey(h => h.CatalogTypeId);
        }
    }
}
