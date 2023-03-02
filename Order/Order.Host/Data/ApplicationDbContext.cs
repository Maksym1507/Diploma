using Order.Host.Data.EntityConfigurations;

namespace Order.Host.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<OrderEntity> Orders { get; set; } = null!;

        public DbSet<OrderDetailsEntity> OrderDetails { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder buider)
        {
            buider.ApplyConfiguration(new OrderEntityTypeConfiguration());
            buider.ApplyConfiguration(new OrderDetailsEntityTypeConfiguration());
        }
    }
}
