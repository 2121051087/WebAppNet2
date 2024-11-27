using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAppNet2.Models.Entities.Carts;
using WebAppNet2.Models.Entities.Catalog;
using WebAppNet2.Models.Entities.Orders;

namespace AppNet2.Models
{
    public class DbNet2Context : IdentityDbContext<ApplicationUser>
    {
        public DbNet2Context(DbContextOptions<DbNet2Context> options) : base(options)
        {
        }

        // Catalog
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ColorSizes> ColorSizes { get; set; }
        public DbSet<Colors> Colors { get; set; }
        public DbSet<Sizes> Sizes { get; set; }

        // Order
        public DbSet<Orders> Orders { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }

        // Cart
        public DbSet<Carts> Carts { get; set; }
        
        public DbSet<CartItem> CartItem { get; set; }


        // Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Categories
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.ToTable("Categories")
                      .HasKey(c => c.CategoryID);
            });

            //  Products
            modelBuilder.Entity<Products>(entity =>
            {
                entity.ToTable("Products")
                      .HasKey(p => p.ProductID);

                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryID)
                      .OnDelete(DeleteBehavior.Cascade);

            });

            // Colors
            modelBuilder.Entity<Colors>(entity =>
            {
                entity.ToTable("Colors")
                      .HasKey(c => c.ColorID);
            });

            //  Sizes
            modelBuilder.Entity<Sizes>(entity =>
            {
                entity.ToTable("Sizes")
                      .HasKey(s => s.SizeID);
            });

            //  ColorSizes
            modelBuilder.Entity<ColorSizes>(entity =>
            {
                entity.ToTable("ColorSizes")
                      .HasKey(cs => cs.ColorSizesID);

                entity.HasOne(cs => cs.Color)
                      .WithMany(c => c.ColorSizes)
                      .HasForeignKey(cs => cs.ColorID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(cs => cs.Sizes)
                      .WithMany(s => s.ColorSizes)
                      .HasForeignKey(cs => cs.SizeID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(cs => cs.Product)
                      .WithMany(p => p.ColorSizes)
                      .HasForeignKey(cs => cs.ProductID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            //  Orders
            modelBuilder.Entity<Orders>(entity =>
            {
                entity.ToTable("Orders")
                      .HasKey(o => o.OrderID);

                //entity.HasOne(o => o.applicationUser)
                //      .WithMany(u => u.Orders)
                //      .HasForeignKey(o => o.UserID)
                //      .OnDelete(DeleteBehavior.Restrict);
            });

            // OrderDetails
            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.ToTable("OrderDetails")
                      .HasKey(od => od.OrderDetailID);

                entity.HasOne(od => od.Orders)
                      .WithMany(o => o.OrderDetails)
                      .HasForeignKey(od => od.OrderID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(od => od.Products)
                      .WithMany(p => p.OrderDetails)
                      .HasForeignKey(od => od.ProductID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(od => od.ColorSizes)
                      .WithMany(cs => cs.OrderDetails)
                      .HasForeignKey(od => od.ColorSizesID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            //  Carts
            modelBuilder.Entity<Carts>(entity =>
            {
                entity.ToTable("Carts")
                      .HasKey(c => c.CartID);
            });

            //  CartItem
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.ToTable("Cart_item")
                      .HasKey(ci => ci.Cart_itemID);

                entity.HasOne(ci => ci.Carts)
                      .WithMany(c => c.cartItems)
                      .HasForeignKey(ci => ci.CartID)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(ci => ci.ColorSizes)
                      .WithMany(cs => cs.cartItems)
                      .HasForeignKey(ci => ci.ColorSizesID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ci => ci.Products)
                      .WithMany(p => p.CartItems)
                      .HasForeignKey(ci => ci.ProductID)
                      .OnDelete(DeleteBehavior.Cascade); // Cascade nếu muốn xóa tất cả CartItems khi Product bị xóa
            });

        }
    }
}
