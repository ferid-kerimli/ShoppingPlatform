using ShoppingPlatform.DAL.Enum;

namespace ShoppingPlatform.DAL.Entity;

public class CheckOut : BaseEntity
{
    public int UserId { get; set; }
    public AppUser AppUser { get; set; }

    public ICollection<BasketItem> BasketItems { get; set; }
    
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; }
    public string BillingAddress { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
}