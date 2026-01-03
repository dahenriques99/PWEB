using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyMedia.Infrastructure.Entities;

namespace MyMedia.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<DeliveryMode> DeliveryModes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        /* ================ PRODUCT CATEGORY ======================== */
        modelBuilder.Entity<ProductCategory>()
            .HasKey(pc => new { pc.ProductId, pc.CategoryId });
        
        modelBuilder.Entity<ProductCategory>()
            .HasOne(pc => pc.Product)
            .WithMany(p => p.ProductCategories)
            .HasForeignKey(pc => pc.ProductId);

        modelBuilder.Entity<ProductCategory>()
            .HasOne(pc => pc.Category)
            .WithMany(c => c.ProductCategories)
            .HasForeignKey(pc => pc.CategoryId);
        
        /* ======================== CATEGORY ============================= */
        modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
            
        /* ======================== ORDER ============================= */
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .IsRequired();
        modelBuilder.Entity<Order>()
            .HasOne(o => o.DeliveryMode)
            .WithMany(dm => dm.Orders)
            .HasForeignKey(o => o.DeliveryId)
            .IsRequired();

        /* ==================== ORDER ITEM ============================ */
        modelBuilder.Entity<OrderItem>()
            .HasKey(cp => new { cp.OrderId, cp.ProductId });
        
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o=> o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .IsRequired();
    
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        /* ======================== SUPPLIER ============================= */
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Supplier)
            .WithMany(u => u.SuppliedProducts)
            .HasForeignKey(p => p.SupplierId)
            .IsRequired();
        
        /* ====================== DELIVERY MODE ========================== */
        modelBuilder.Entity<DeliveryMode>()
            .HasData(new DeliveryMode { Id = 1, Name = "Delivery" });
        modelBuilder.Entity<DeliveryMode>()
            .HasData(new DeliveryMode { Id = 2, Name = "Pickup" });
        modelBuilder.Entity<DeliveryMode>()
            .HasData(new DeliveryMode { Id = 3, Name = "Store Pickup" });
        
        
    }
}
