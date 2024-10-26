using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Implementation.Services
{
    public class ProductServices : IProductServices
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IImageService _imageService;
        private readonly ILogger<ProductServices> _logger;

        public ProductServices(ApplicationDbContext dbContext, IImageService imageService, ILogger<ProductServices> logger)
        {
            _dbContext = dbContext;
            _imageService = imageService;
            _logger = logger;
        }

        public async Task<BaseResponse<Guid>> CreateProduct(CreateProduct request)
        {
            _logger.LogInformation("Creating product with name: {ProductName}", request.Name);

            if (request == null)
            {
                _logger.LogWarning("CreateProduct request is null.");
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Invalid product data.",
                    Hasherror = true
                };
            }

            try
            {
                var product = new Product
                {
                    Id = request.Id,
                    Name = request.Name,
                    Price = request.Price
                };

                _dbContext.Products.Add(product);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    //await _imageService.AddImagesAsync(request.Images, product.Id);
                    _logger.LogInformation("Product created successfully with name: {ProductName}", request.Name);

                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = "Product Created Successfully",
                        Data = product.Id
                    };
                }
                else
                {
                    _logger.LogWarning("Failed to create product with name: {ProductName}", request.Name);
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Failed to Create Product",
                        Hasherror = true
                    };
                }
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
                    Price = x.Price,
                    Images = x.Images.Select(x => new Dto.ImageDto
                    {
                        Id = x.Id,
                        ImagePath = x.ImagePath,
                    }).ToList()
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
            _logger.LogInformation("Retrieving product with ID: {ProductId}", id);
            var product = await _dbContext.Products
                .Where(x => x.Id == id)
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Images = x.Images.Select(x => new Dto.ImageDto
                    {
                        Id = x.Id,
                        ImagePath = x.ImagePath,
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (product != null)
            {
                _logger.LogInformation("Product retrieved successfully with ID: {ProductId}", id);
                return new BaseResponse<ProductDto>
                {
                    Success = true,
                    Message = "Product Retrieved Successfully",
                    Data = product
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
                        Price = product.Price,
                        Images = product.Images.Select(x => new Dto.ImageDto
                        {
                            Id = x.Id,
                            ImagePath = x.ImagePath,
                        }).ToList()
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
                    Price = x.Price,
                    Images = x.Images.Select(x => new Dto.ImageDto
                    {
                        Id = x.Id,
                        ImagePath = x.ImagePath,
                    }).ToList()
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
                var product = await _dbContext.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
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
                if (request.Images != null )
                {
                    _logger.LogInformation("Updating images for product with ID: {ProductId}", id);
                    _dbContext.Images.RemoveRange(product.Images);
                    await _dbContext.SaveChangesAsync();
                    foreach (var imageDto in request.Images)
                    {
                        var newImage = new Models.Entity.Images 
                        {
                            Id = Guid.NewGuid(),
                            ImagePath = imageDto.ImagePath,
                            //ProductId = product.Id
                        };
                        _dbContext.Images.Add(newImage);
                    }
                }

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

    }
}
