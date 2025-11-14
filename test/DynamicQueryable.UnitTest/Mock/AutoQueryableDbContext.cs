using DynamicQueryable.UnitTest.Mock.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace DynamicQueryable.UnitTest.Mock
{
    public class DynamicQueryableDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var loggerFactory = LoggerFactory.Create(builder => {
                builder.AddConsole();
            });
            optionsBuilder.UseLoggerFactory(loggerFactory);
            optionsBuilder.UseInMemoryDatabase("test");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProductExtension>().HasQueryFilter(b => !b.IsDeleted);

            builder.Entity<Product>().OwnsMany(x => x.MyComplexClass).WithOwner(x => x.Product).HasForeignKey(x => x.ProductId);
            builder.Entity<Product>().HasKey(x => x.ProductId);

        }


        public DbSet<Product> Product { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductModel> ProductModel { get; set; }
        public DbSet<SalesOrderDetail> SalesOrderDetail { get; set; }
        public DbSet<ComplexClass> MyComplexClass { get; set; }
    }
}