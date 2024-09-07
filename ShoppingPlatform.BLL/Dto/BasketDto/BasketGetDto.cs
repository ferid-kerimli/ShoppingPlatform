namespace ShoppingPlatform.BLL.Dto.BasketDto;

public class BasketGetDto
{
    public int UserId { get; set; }
    public List<BasketItemGetDto> BasketItems { get; set; } 
}