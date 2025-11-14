using DynamicQueryable.Sample.EfCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace DynamicQueryable.Sample.EfCore.Contexts
{
    public class DynamicQueryableDbContext : DbContext
    {
        public DynamicQueryableDbContext(DbContextOptions<DynamicQueryableDbContext> options)
            : base(options)
        {
            
        }

        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<ProductModel> ProductModel { get; set; }
        public virtual DbSet<SalesOrderDetail> SalesOrderDetail { get; set; }
    }
}