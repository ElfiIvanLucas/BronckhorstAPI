using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class BronckhorstDbContext : DbContext
    {
        public BronckhorstDbContext(DbContextOptions<BronckhorstDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductCategory>()
                .HasKey(productCategory => new { productCategory.ProductId, productCategory.CategoryId });

            modelBuilder.Entity<ProductCategory>()
                .HasOne(productCategory => productCategory.Product)
                .WithMany(product => product.ProductCategories)
                .HasForeignKey(productCategory => productCategory.ProductId);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(productCategory => productCategory.Category)
                .WithMany(category => category.ProductCategories)
                .HasForeignKey(productCategory => productCategory.CategoryId);

            modelBuilder.Entity<Category>()
                .HasOne(category => category.Parent)
                .WithMany(category => category.Children)
                .HasForeignKey(category => category.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        #region DbSets
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerOrder>CustomerOrders{ get; set; }
        public DbSet<OrderLine> OrderLines{ get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Product> Products  { get; set; }
        public DbSet<ProductCategory> ProductCategories   { get; set; }
        #endregion DbSets   
    }
}
   
