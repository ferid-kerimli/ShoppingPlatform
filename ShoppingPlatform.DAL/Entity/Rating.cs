using System.ComponentModel.DataAnnotations;

namespace ShoppingPlatform.DAL.Entity;

public class Rating : BaseEntity
{
    [Range(1,5)] 
    public int Value { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int UserId { get; set; }
    public AppUser AppUser { get; set; }
}