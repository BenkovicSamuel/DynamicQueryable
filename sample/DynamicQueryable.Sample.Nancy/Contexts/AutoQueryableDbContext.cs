using DynamicQueryable.Sample.Nancy.Entities;
using Microsoft.EntityFrameworkCore;

namespace DynamicQueryable.Sample.Nancy.Contexts
{
    public class DynamicQueryableDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("ProductsInMemory");
        }

        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<ProductModel> ProductModel { get; set; }
        public virtual DbSet<SalesOrderDetail> SalesOrderDetail { get; set; }
    }
}