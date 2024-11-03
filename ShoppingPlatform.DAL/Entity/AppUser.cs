using Microsoft.AspNetCore.Identity;

namespace ShoppingPlatform.DAL.Entity;

public class AppUser : IdentityUser<int>
{
    public ICollection<Review> Reviews { get; set; }
    public ICollection<WishList> WishLists { get; set; }
    public ICollection<Basket> Baskets { get; set; }    
    public ICollection<Product> Products { get; set; }
}