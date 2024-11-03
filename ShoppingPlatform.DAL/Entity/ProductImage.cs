namespace ShoppingPlatform.DAL.Entity;

public class ProductImage : BaseEntity
{
    public string FilePath { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}