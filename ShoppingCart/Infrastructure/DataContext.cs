using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;

namespace ShoppingCart.Infrastructure
{
    public class DataContext : IdentityDbContext<AppUser>
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<DetailSale> DetailsSale{ get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Category>()
            //    .HasMany(category => category.Products)
            //    .WithOne(product => product.Category)
            //    .HasForeignKey(product => product.CategoryId)
            //    .IsRequired();

            //builder.Entity<Product>()
            //    .HasOne(product => product.Category)
            //    .WithMany(category => category.Products)
            //    .HasForeignKey(product => product.CategoryId);

        }
    }
}
