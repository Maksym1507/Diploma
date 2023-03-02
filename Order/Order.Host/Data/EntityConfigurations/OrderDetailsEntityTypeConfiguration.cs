using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Order.Host.Data.EntityConfigurations
{
    public class OrderDetailsEntityTypeConfiguration : IEntityTypeConfiguration<OrderDetailsEntity>
    {
        public void Configure(EntityTypeBuilder<OrderDetailsEntity> builder)
        {
            builder.ToTable("OrderDetails");

            builder.HasKey(h => h.Id);

            builder.Property(p => p.Id)
                .UseHiLo("order_details_hilo")
                .IsRequired();

            builder.Property(h => h.OrderId).IsRequired();
            builder.Property(p => p.Count).IsRequired();

            builder.HasOne(h => h.Order)
                .WithMany(w => w.OrderDetails)
                .HasForeignKey(h => h.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
