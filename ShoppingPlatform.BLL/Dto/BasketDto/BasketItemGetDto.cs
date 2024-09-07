namespace ShoppingPlatform.BLL.Dto.BasketDto;

public class BasketItemGetDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }
    public int Quantity { get; set; }
}