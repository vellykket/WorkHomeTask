using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using HomeTask.Models;

namespace HomeTask.Models
{
    public class HomeTaskContext : DbContext
    {
        public HomeTaskContext(DbContextOptions<HomeTaskContext> options) : base(options) {}

        public  DbSet<Product> Products { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public  DbSet<Currency> Currencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Storage>()
                .HasMany(storage => storage.Products)
                .WithMany(product => product.Storages)
                .UsingEntity<ProductsInStock>(
                    prodInStock => prodInStock
                        .HasOne(productsInStock => productsInStock.Product)
                        .WithMany(product => product.ProductsInStocks)
                        .HasForeignKey(productsInStock => productsInStock.ProductId),
                    prodInStock => prodInStock
                        .HasOne(productsInStock => productsInStock.Storage)
                        .WithMany(storage => storage.ProductsInStocks)
                        .HasForeignKey(productsInStock => productsInStock.StorageId),
                    prodInStock =>
                    {
                        prodInStock.Property(productsInStock => productsInStock.Count);
                        prodInStock.HasKey(t => new {t.StorageId, t.ProductId});
                        prodInStock.ToTable("ProductInStock");
                    });
        }

        public DbSet<HomeTask.Models.ProductsInStock> ProductsInStock { get; set; }
    }
}