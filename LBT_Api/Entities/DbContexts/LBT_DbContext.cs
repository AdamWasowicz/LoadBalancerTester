using Microsoft.EntityFrameworkCore;

namespace LBT_Api.Entities
{
    public class LBT_DbContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSold> ProductsSold { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Worker> Workers { get; set; }

        
        public LBT_DbContext(DbContextOptions options) : base(options) 
        {
            // For PostgreSQL compatibility
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}
