using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingPlatform.DAL.Entity;
using ShoppingPlatform.DAL.Extensions;

namespace ShoppingPlatform.DAL.Context;

public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Seed();

        builder.Entity<Basket>()
            .Property(b => b.TotalPrice)
            .HasColumnType("decimal(18, 2)");
        
        builder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18, 2)");

        builder.Entity<Product>()
            .Property(p => p.AverageRating)
            .HasColumnType("decimal(18, 1)");

        builder.Entity<CheckOut>()
            .Property(c => c.TotalAmount)
            .HasColumnType("decimal(18, 2)");
        
        // Product
        builder.Entity<Product>()
            .HasMany(p => p.ProductImages)
            .WithOne(pi => pi.Product)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Product>()
            .HasOne(p => p.AppUser)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Product>()
            .HasMany(p => p.BasketItems)
            .WithOne(bi => bi.Product)
            .HasForeignKey(bi => bi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Basket
        builder.Entity<Basket>()
            .HasOne(b => b.AppUser)
            .WithMany(u => u.Baskets)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Rating
        builder.Entity<Rating>()
            .HasOne(r => r.AppUser)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Review
        builder.Entity<Review>()
            .HasOne(r => r.AppUser)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict); 

        builder.Entity<Review>()
            .HasOne(r => r.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        
        // BasketItem
        builder.Entity<BasketItem>()
            .HasOne(b => b.Basket)
            .WithMany(b => b.BasketItems)
            .HasForeignKey(b => b.BasketId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<BasketItem>()
            .HasOne(b => b.Product)
            .WithMany(b => b.BasketItems)
            .HasForeignKey(b => b.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Wishlist
        builder.Entity<WishList>()
            .HasOne(w => w.AppUser)
            .WithMany(u => u.WishLists)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // WishlistItem
        builder.Entity<WishlistItem>()
            .HasOne(w => w.WishList)
            .WithMany(b => b.WishlistItems)
            .HasForeignKey(b => b.WishListId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<WishlistItem>()
            .HasOne(b => b.Product)
            .WithMany(b => b.WishlistItems)
            .HasForeignKey(b => b.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CheckOut> CheckOuts { get; set; }
    public DbSet<Product> Products { get; set; } 
    public DbSet<ProductImage> ProductImages { get; set; } 
    public DbSet<Rating> Ratings { get; set; } 
    public DbSet<Review> Reviews { get; set; } 
    public DbSet<WishList> WishLists { get; set; }
    public DbSet<WishlistItem> WishlistItems { get; set; }
}