namespace ShoppingPlatform.DAL.Entity;

public class WishList : BaseEntity
{
    public int UserId { get; set; }
    public AppUser AppUser { get; set; }

    public ICollection<WishlistItem> WishlistItems { get; set; }
}