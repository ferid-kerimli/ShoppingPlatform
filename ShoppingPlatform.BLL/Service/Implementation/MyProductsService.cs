using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Serilog;
using ShoppingPlatform.BLL.Dto.ProductDto;
using ShoppingPlatform.BLL.Enum;
using ShoppingPlatform.BLL.Response;
using ShoppingPlatform.BLL.Service.Abstraction;
using ShoppingPlatform.DAL.Abstraction;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.BLL.Service.Implementation;

public class MyProductsService : IMyProductsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MyProductsService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMapper mapper, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse<List<ProductGetDto>>> GetMyAllProducts()
    {
        Log.Logger.Information(nameof(GetMyAllProducts) + "GetAllCategories called");
        var response = new ApiResponse<List<ProductGetDto>>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(GetMyAllProducts) + "User not logged in: {email}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not found", 404);
                Log.Logger.Warning(nameof(GetMyAllProducts) + "User not found: {user}", user);
                return response;
            }
            
            // get user products
            var products = await _unitOfWork.ProductRepository.GetProductsByUserId(user.Id);

            if (products.Count == 0)
            {
                response.Failure("You don't have any products", 404);
                Log.Logger.Warning(nameof(GetMyAllProducts) + "User don't have any products: {user}", user);
                return response;
            }

            var productGetDtoS = new List<ProductGetDto>();

            if (true)
            {
                foreach (var product in products)
                {
                    var productGetDto = new ProductGetDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        UserId = product.UserId,
                        Files = new List<string>()
                    };

                    foreach (var image in product.ProductImages)
                    {
                        if (File.Exists(image.FilePath))
                        {
                            var imageBytes = await File.ReadAllBytesAsync(image.FilePath);
                            var base64String = Convert.ToBase64String(imageBytes);
                            productGetDto.Files.Add(base64String);
                        }
                        else
                        {
                            response.Failure("Image's filepath not found", 404);
                            Log.Logger.Warning(nameof(GetMyAllProducts) + "Image's filepath not found: {file}", image.FilePath);
                            return response;
                        }
                    }
                    
                    productGetDtoS.Add(productGetDto);
                }
            }
            
            response.Success(productGetDtoS, 200);
            Log.Logger.Information(nameof(GetMyAllProducts) + "GetAllProducts completed successfully");
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(GetMyAllProducts) + "Error happened while getting all categories: {Message}", e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<ProductGetDto>> GetMyProductById(int id)
    {
        Log.Logger.Information(nameof(GetMyProductById) + "GetProductById called");
        var response = new ApiResponse<ProductGetDto>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(GetMyProductById) + "User not logged in: {email}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not found", 404);
                Log.Logger.Warning(nameof(GetMyProductById) + "User not found: {user}", user);
                return response;
            }

            var product = await _unitOfWork.ProductRepository.GetProductByUserId(user.Id, id);

            if (product == null)
            {
                response.Failure("Product not found", 404);
                Log.Logger.Warning(nameof(GetMyProductById) + "Product not found with this id: {id}", id);
                return response;
            }

            var productEntity = _mapper.Map<ProductGetDto>(product);
            
            if (product.ProductImages != null)
            {
                productEntity.Files = product.ProductImages.Select(pi => pi.FilePath).ToList();
            }
            
            response.Success(productEntity, 200);
            Log.Logger.Information(nameof(GetMyProductById) + "GetProductById completed successfully for this id: {id}", user.Id);
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(GetMyProductById) + "Error happened while getting product by id: {message}", e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<List<ProductGetDto>>> GetMyProductsByCategoryId(int categoryId)
    {
        Log.Logger.Information(nameof(GetMyProductsByCategoryId) + "GetProductByCategoryId called");
        var response = new ApiResponse<List<ProductGetDto>>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(GetMyProductsByCategoryId) + "User not logged in with this email: {email}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not found", 404);
                Log.Logger.Warning(nameof(GetMyProductsByCategoryId) + "User not found with this email: {email}", userEmail);
                return response;
            }

            var products = await _unitOfWork.ProductRepository.GetProductsByUserCategoryId(user.Id, categoryId);
            
            if (products == null)
            {
                response.Failure("No products found for this category", 404);
                Log.Logger.Warning(nameof(GetMyProductsByCategoryId) + " No products found with this category id: {categoryId}", categoryId);
                return response;
            }
            
            var productGetDtoS = products.Select(product => {
                var productDto = _mapper.Map<ProductGetDto>(product);
                if (product.ProductImages != null)
                {
                    productDto.Files = product.ProductImages.Select(pi => pi.FilePath).ToList();
                }
                return productDto;
            }).ToList();

            response.Success(productGetDtoS, 200);
            Log.Logger.Information(nameof(GetMyProductsByCategoryId) + " Products retrieved successfully");
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(GetMyProductsByCategoryId) + "Error happened while getting product by id: {message}", e.Message);
            throw;
        }

        return response;
    }


    public async Task<ApiResponse<ProductCreateDto>> CreateProduct(ProductCreateDto productCreateDto)
    {
        Log.Logger.Information(nameof(CreateProduct) + "Creating product");
        var response = new ApiResponse<ProductCreateDto>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            
            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(CreateProduct) + "User not logged in: {userEmail}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not found", 401);
                Log.Logger.Warning(nameof(CreateProduct) + "User not found: {User}", user);
                return response;
            }

            var productEntity = _mapper.Map<Product>(productCreateDto);
            
            productEntity.UserId = user.Id;
            productEntity.Price = productCreateDto.Price;
            productEntity.CreateDate = DateTime.Now;
            productEntity.CategoryId = productCreateDto.CategoryId;

            var productImages = new List<ProductImage>();
            
            foreach (var file in productCreateDto.Files)
            {
                var productImage = new ProductImage();
            
                var fileName = Guid.NewGuid() + file.FileName;
                const string folderName = nameof(FolderNames.ProductImages);
                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, folderName);
                var filePath = Path.Combine(folderPath, fileName);
                
                if (productCreateDto is { Files: null }) continue;
            
                await using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            
                productImage.FilePath = filePath;
                productImage.ProductId = productEntity.Id;
            
                productImages.Add(productImage);
            }
            
            productEntity.ProductImages = productImages;
            await _unitOfWork.ProductRepository.Create(productEntity);


            var result = await _unitOfWork.Commit();

            if (result > 0)
            {
                response.Success(productCreateDto, 201);
            }

            Log.Logger.Information(nameof(CreateProduct) + " Product created successfully");
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(CreateProduct) + " Error happened while creating product {Message}", e.Message);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<ProductUpdateDto>> UpdateProduct(ProductUpdateDto productUpdateDto)
    {
        Log.Logger.Information(nameof(UpdateProduct) + "UpdateProduct called");
        var response = new ApiResponse<ProductUpdateDto>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(UpdateProduct) + "User not logged in with this email: {email}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not found", 404);
                Log.Logger.Warning(nameof(UpdateProduct) + "User not found: {user}", user);
                return response;
            }

            var product = await _unitOfWork.ProductRepository.GetProductByUserId(user.Id, productUpdateDto.Id);

            if (product == null)
            {
                response.Failure("Product not found", 404);
                Log.Logger.Warning(nameof(UpdateProduct) + "Product not found with this id: {id}", productUpdateDto.Id);
                return response;
            }

            product.Name = productUpdateDto.Name;
            product.Description = productUpdateDto.Description;
            product.Price = productUpdateDto.Price;

            _unitOfWork.ProductRepository.Update(product);

            var result = await _unitOfWork.Commit();
            
            if (result > 0)
            {
                response.Success(productUpdateDto, 200);
                Log.Logger.Information(nameof(UpdateProduct) + " Product updated successfully");
            }
            else
            {
                response.Failure("Product update failed", 500);
                Log.Logger.Error(nameof(UpdateProduct) + " Product update failed for id: {id}", productUpdateDto.Id);
            }
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(UpdateProduct) + "Error happened while updating product: {message}", e.Message);
            throw;
        }

        return response;
    }
    
    public async Task<ApiResponse<bool>> DeleteProduct(int productId)
    {
        Log.Logger.Information(nameof(DeleteProduct) + " DeleteProduct called");
        var response = new ApiResponse<bool>();

        try
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                response.Failure("User not logged in", 401);
                Log.Logger.Warning(nameof(DeleteProduct) + " User not logged in: {email}", userEmail);
                return response;
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                response.Failure("User not found", 404);
                Log.Logger.Warning(nameof(DeleteProduct) + " User not found: {user}", user);
                return response;
            }

            var product = await _unitOfWork.ProductRepository.GetProductByUserId(user.Id, productId);

            if (product == null)
            {
                response.Failure("Product not found", 404);
                Log.Logger.Warning(nameof(DeleteProduct) + " Product not found with this id: {id}", productId);
                return response;
            }

            _unitOfWork.ProductRepository.Delete(product);
            var result = await _unitOfWork.Commit();

            if (result > 0)
            {
                response.Success(true, 200);
                Log.Logger.Information(nameof(DeleteProduct) + " Product deleted successfully");
            }
            else
            {
                response.Failure("Product deletion failed", 500);
                Log.Logger.Error(nameof(DeleteProduct) + " Product deletion failed for id: {id}", productId);
            }
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(DeleteProduct) + " Error happened while deleting product: {message}", e.Message);
            throw;
        }

        return response;
    }
}