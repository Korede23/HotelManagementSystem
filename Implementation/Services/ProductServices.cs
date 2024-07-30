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

        public ProductServices(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<BaseResponse<Guid>> CreateProduct(CreateProduct request)
        {
            try
            {
                if (request != null)
                {
                    var items = new Product
                    {
                        Id = request.Id,
                        Name = request.Name,
                        Price = request.Price
                    };
                    _dbContext.Products.Add(items);
                }

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = "Product Created Successfully",

                    };
                }
                else
                {
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Fail To Create Product",
                        Hasherror = true
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Fail To Create Product",
                    Hasherror = true
                };
            }


        }


        public async Task<List<ProductDto>> GetProduct()
        {
            return _dbContext.Products
                .Select(x => new ProductDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price
                }).ToList();
        }


        public async Task<BaseResponse<Guid>> DeleteProductAsync(Guid Id)
        {
            try
            {
                var item = await _dbContext.Products.FirstOrDefaultAsync();

                if (item != null)
                {
                    _dbContext.Products.Remove(item);
                }
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = "Product has been deleted Succesfully"
                    };
                }
                else
                {
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Failed to delete This Product",
                        Hasherror = true
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Failed to delete This Product",
                    Hasherror = true
                };
            }

        }

        public async Task<BaseResponse<ProductDto>> GetAllProductsByIdAsync(Guid Id)
        {

            var item = await _dbContext.Products
                .Where(x => x.Id == Id)
                .Select(x => new ProductDto
                {
                    Name = x.Name,
                    Price = x.Price,
                }).FirstOrDefaultAsync();
            if (item != null)
            {
                return new BaseResponse<ProductDto>
                {
                    Success = true,
                    Message = "Products Retrieved Succesfully",
                    Data = item
                };
            }
            else
            {
                return new BaseResponse<ProductDto>
                {
                    Success = false,
                    Message = "Retrieved Failed",
                    Hasherror = true
                };
            }

        }


        public async Task<BaseResponse<ProductDto>> GetProductAsync(Guid Id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == Id);
            if (product != null)
            {
                return new BaseResponse<ProductDto>
                {
                    Message = "",
                    Success = true,
                    Data = new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                    }

                };
            }
            return new BaseResponse<ProductDto>
            {
                Success = false,
                Message = "",
            };
        }

        public async Task<BaseResponse<IList<ProductDto>>> GetAllProductAsync()
        {

            var item = await _dbContext.Products
                .Select(x => new ProductDto
                {
                    Name = x.Name,
                    Price = x.Price,
                }).ToListAsync();
            if (item != null)
            {
                return new BaseResponse<IList<ProductDto>>
                {
                    Success = true,
                    Message = "Products Retrieved Succesfully",
                    Data = item
                };
            }
            else
            {
                return new BaseResponse<IList<ProductDto>>
                {
                    Success = false,
                    Message = "Retrieved Failed",
                    Hasherror = true
                };
            }
        }


        //public async Task<BaseResponse<ProductDto>> UpdateProduct(Guid Id, UpdateProduct request)
        //{
        //    try
        //    {
        //        var item = await _dbContext.Products.FirstOrDefaultAsync();
        //        if (item == null)
        //        {
        //            return new BaseResponse<ProductDto>
        //            {
        //                Success = false,
        //                Message = "Failed to Update Product ,there was an error in the updating process.",
        //                Hasherror = true
        //            };
        //        }

        //        //item.Id = request.Id;
        //        item.Name = request.Name;
        //        item.Price = request.Price;
        //        _dbContext.Products.Update(item);
        //        if (await _dbContext.SaveChangesAsync() > 0)
        //        {
        //            return new BaseResponse<ProductDto>
        //            {
        //                Success = true,
        //                Message = $"Product with ID {Id} Updated successfully."
        //            };
        //        }
        //        else
        //        {
        //            return new BaseResponse<ProductDto>
        //            {
        //                Success = false,
        //                Message = "Failed to Update Product ,there was an error in the updating process.",
        //                Hasherror = true
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new BaseResponse<ProductDto>
        //        {
        //            Success = false,
        //            Message = "Failed to Update Product ,there was an error in the updating process.",
        //            Hasherror = true
        //        };
        //    }


        //}


        public async Task<BaseResponse<ProductDto>> UpdateProduct(Guid id, UpdateProduct request)
        {
            try
            {
                var item = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (item == null)
                {
                    return new BaseResponse<ProductDto>
                    {
                        Success = false,
                        Message = "Product not found.",
                        Hasherror = true
                    };
                }

                item.Name = request.Name;
                item.Price = request.Price;

                _dbContext.Products.Update(item);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return new BaseResponse<ProductDto>
                    {
                        Success = true,
                        Message = $"Product with ID {id} updated successfully."
                    };
                }
                else
                {
                    return new BaseResponse<ProductDto>
                    {
                        Success = false,
                        Message = "Failed to update product.",
                        Hasherror = true
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<ProductDto>
                {
                    Success = false,
                    Message = "An error occurred while updating the product.",
                    Hasherror = true
                };
            }
        }

    }
}
