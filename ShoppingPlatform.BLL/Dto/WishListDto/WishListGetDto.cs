namespace ShoppingPlatform.BLL.Dto.WishListDto;

public class WishListGetDto
{
    public int UserId { get; set; }
    public List<WishListItemGetDto> WishlistItems { get; set; }
}