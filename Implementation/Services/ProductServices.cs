using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Implementation.Services
{
    public class ProductServices : IProductServices
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IImageService _imageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ProductServices> _logger;

        public ProductServices(ApplicationDbContext dbContext, IImageService imageService,
          IHttpContextAccessor httpContextAccessor,
         UserManager<User> userManager, ILogger<ProductServices> logger)
        {
            _dbContext = dbContext;
            _imageService = imageService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<BaseResponse<Guid>> CreateProduct(CreateProduct request)
        {
            _logger.LogInformation("Creating product with name: {ProductName}", request.Name);

            if (request == null)
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Invalid product data.",
                    Hasherror = true
                };
            }
            var checkIfProductExist = await _dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (checkIfProductExist != null)
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Product already exists"
                };
            }
            var userPrincipal = _httpContextAccessor.HttpContext?.User;
            if (userPrincipal == null)
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "User not authenticated"
                };
            }

            var user = await _userManager.GetUserAsync(userPrincipal);
            if (user == null)
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            try
            {
                var product = new Product
                {
                    Id = request.Id,
                    Name = request.Name,
                    Price = request.Price,
                    CreatedBy = user.UserName

                };

                if (request.Images != null && request.Images.Count > 0)
                {

                    var imageFilenames = await _imageService.AddProductImagesAsync(request.Images);
                    product.ImageUrls = string.Join(",", imageFilenames);
                }

                _dbContext.Products.Add(product);
                await _dbContext.SaveChangesAsync();

                return new BaseResponse<Guid>
                {
                    Success = true,
                    Message = "Product Created Successfully",
                    Data = product.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the product with name: {ProductName}", request.Name);
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Failed to Create Product",
                    Hasherror = true
                };
            }
        }




        public async Task<List<ProductDto>> GetProduct()
        {
            _logger.LogInformation("Retrieving all products as list");
            return await _dbContext.Products
                .Select(x => new ProductDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price
                }).ToListAsync();
        }

        public async Task<BaseResponse<Guid>> DeleteProductAsync(Guid id)
        {
            _logger.LogInformation("Deleting product with ID: {ProductId}", id);
            try
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

                if (product != null)
                {
                    _dbContext.Products.Remove(product);
                }

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    _logger.LogInformation("Product deleted successfully with ID: {ProductId}", id);
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = "Product has been deleted Successfully"
                    };
                }
                else
                {
                    _logger.LogWarning("Failed to delete product with ID: {ProductId}", id);
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Failed to delete this product",
                        Hasherror = true
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the product with ID: {ProductId}", id);
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Failed to delete this product",
                    Hasherror = true
                };
            }
        }

        public async Task<BaseResponse<ProductDto>> GetAllProductsByIdAsync(Guid id)
        {
            _logger.LogInformation("Retrieving product name, ID, and images with ID: {ProductId}", id);

            var product = await _dbContext.Products
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImageUrls = x.ImageUrls,
                    Price = x.Price,

                })
                .FirstOrDefaultAsync();

            if (product != null)
            {

                var productDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    ImageUrls = string.IsNullOrEmpty(product.ImageUrls)
                                ? new List<string>()
                                : product.ImageUrls.Split(',').ToList()
                };

                _logger.LogInformation("Product name, ID, and images retrieved successfully with ID: {ProductId}", id);
                return new BaseResponse<ProductDto>
                {
                    Success = true,
                    Message = "Product Retrieved Successfully",
                    Data = productDto
                };
            }
            else
            {
                _logger.LogWarning("Failed to retrieve product with ID: {ProductId}", id);
                return new BaseResponse<ProductDto>
                {
                    Success = false,
                    Message = "Failed to Retrieve Product",
                    Hasherror = true
                };
            }
        }



        public async Task<BaseResponse<ProductDto>> GetProductAsync(Guid id)
        {
            _logger.LogInformation("Retrieving product with ID: {ProductId}", id);
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product != null)
            {
                _logger.LogInformation("Product retrieved successfully with ID: {ProductId}", id);
                return new BaseResponse<ProductDto>
                {
                    Success = true,
                    Data = new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price
                    }
                };
            }
            _logger.LogWarning("Failed to retrieve product with ID: {ProductId}", id);
            return new BaseResponse<ProductDto>
            {
                Success = false,
                Message = "Product not found",
            };
        }

        public async Task<BaseResponse<IList<ProductDto>>> GetAllProductAsync()
        {
            _logger.LogInformation("Retrieving all products");
            var products = await _dbContext.Products
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price
                }).ToListAsync();

            if (products != null)
            {
                _logger.LogInformation("Products retrieved successfully");
                return new BaseResponse<IList<ProductDto>>
                {
                    Success = true,
                    Message = "Products Retrieved Successfully",
                    Data = products
                };
            }
            else
            {
                _logger.LogWarning("No products found");
                return new BaseResponse<IList<ProductDto>>
                {
                    Success = false,
                    Message = "Failed to Retrieve Products",
                    Hasherror = true
                };
            }
        }

        public async Task<BaseResponse<ProductDto>> UpdateProduct(Guid id, UpdateProduct request)
        {
            _logger.LogInformation("Updating product with ID: {ProductId}", id);
            try
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product == null)
                {
                    _logger.LogWarning("Product not found with ID: {ProductId}", id);
                    return new BaseResponse<ProductDto>
                    {
                        Success = false,
                        Message = "Product not found",
                        Hasherror = true
                    };
                }

                // Update product details
                product.Name = request.Name;
                product.Price = request.Price;

                _dbContext.Products.Update(product);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Product updated successfully with ID: {ProductId}", id);
                    return new BaseResponse<ProductDto>
                    {
                        Success = true,
                        Message = $"Product with ID {id} updated successfully"
                    };
                }
                else
                {
                    _logger.LogWarning("Failed to update product with ID: {ProductId}", id);
                    return new BaseResponse<ProductDto>
                    {
                        Success = false,
                        Message = "Failed to update product",
                        Hasherror = true
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the product with ID: {ProductId}", id);
                return new BaseResponse<ProductDto>
                {
                    Success = false,
                    Message = "An error occurred while updating the product",
                    Hasherror = true
                };
            }
        }


        public async Task<PaginatedResponse<List<ProductDto>>> GetAllProductsByPaginationAsync(
                                                 int pageNumber,
                                                 int pageSize,
                                                 decimal price,
                                                 string searchTerm = null
)
        {
            _logger.LogInformation("Retrieving all products with pagination");

            try
            {
                IQueryable<Product> query = _dbContext.Products;

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(x => x.Name.Contains(searchTerm));
                }

                if (price > 0)
                {
                    query = query.Where(x => x.Price == price);
                }

                var totalRecords = await query.CountAsync();
                if (totalRecords == 0)
                {
                    _logger.LogInformation("No products found matching the search criteria.");
                    return new PaginatedResponse<List<ProductDto>>
                    {
                        Success = true,
                        Message = "No products found matching the search criteria.",
                        Data = new List<ProductDto>(),
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        TotalRecords = 0
                    };
                }

                var products = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var productDtos = products.Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    ImageUrls = string.IsNullOrEmpty(x.ImageUrls)
                                ? new List<string>()
                                : x.ImageUrls.Split(',').ToList()
                }).ToList();

                _logger.LogInformation("Products retrieved successfully with pagination");

                return new PaginatedResponse<List<ProductDto>>
                {
                    Success = true,
                    Message = "Products retrieved successfully.",
                    Data = productDtos,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = totalRecords
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving products with pagination");
                return new PaginatedResponse<List<ProductDto>>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }



    }
}
