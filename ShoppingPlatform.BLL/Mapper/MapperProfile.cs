using AutoMapper;
using ShoppingPlatform.BLL.Dto.AccountDto;
using ShoppingPlatform.BLL.Dto.CategoryDto;
using ShoppingPlatform.BLL.Dto.ProductDto;
using ShoppingPlatform.BLL.Dto.RoleDto;
using ShoppingPlatform.BLL.Dto.UserDto;
using ShoppingPlatform.BLL.Dto.WishListDto;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.BLL.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<RegisterDto, AppUser>()            
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ReverseMap();
        CreateMap<UserGetDto, AppUser>().ReverseMap();
        
        // Role
        CreateMap<RoleGetDto, AppRole>().ReverseMap();

        // Product
        CreateMap<ProductGetDto, Product>().ReverseMap();
        CreateMap<ProductCreateDto, Product>().ReverseMap();
        CreateMap<ProductUpdateDto, Product>().ReverseMap();
        
        // Category
        CreateMap<CategoryGetDto, Category>().ReverseMap();
        CreateMap<CategoryCreateDto, Category>().ReverseMap();
        CreateMap<CategoryUpdateDto, Category>().ReverseMap();
        
        // Wishlist
        CreateMap<WishListGetDto, WishList>().ReverseMap();
        CreateMap<WishListItemGetDto, WishList>().ReverseMap();
        CreateMap<AddProductToWishListDto, WishList>().ReverseMap();
    }
}