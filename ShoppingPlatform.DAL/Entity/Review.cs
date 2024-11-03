namespace ShoppingPlatform.DAL.Entity;

public class Review : BaseEntity
{
    public string Content { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int UserId { get; set; }
    public AppUser AppUser { get; set; }
}