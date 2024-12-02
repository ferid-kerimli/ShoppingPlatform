using System.ComponentModel.DataAnnotations;

namespace ShoppingPlatform.DAL.Entity;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public DateTime CreateDate { get; set; }
    public decimal? AverageRating { get; set; }  

    public ICollection<Rating> Ratings { get; set; }
    
    public ICollection<Review> Reviews { get; set; }

    public int UserId { get; set; }
    public AppUser AppUser { get; set; }
    
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public ICollection<BasketItem> BasketItems { get; set; }
    public ICollection<WishlistItem> WishlistItems { get; set; }
    public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
}