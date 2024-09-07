namespace ShoppingPlatform.DAL.Entity;

public class Basket : BaseEntity
{
    public int UserId { get; set; }
    public AppUser AppUser { get; set; }

    public DateTime CreationDate { get; set; }
    public decimal TotalPrice { get; set; }
    
    public ICollection<BasketItem> BasketItems { get; set; }
}