using Microsoft.AspNetCore.Http;

namespace ShoppingPlatform.BLL.Dto.ProductDto;

public class ProductCreateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public List<IFormFile> Files { get; set; }
    public int CategoryId { get; set; } 
}