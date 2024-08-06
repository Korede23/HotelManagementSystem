﻿using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace HMS.Implementation.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IProductServices _productServices;
        private readonly ILogger<OrderServices> _logger;

        public OrderServices(ApplicationDbContext dbContext, IProductServices productServices, ILogger<OrderServices> logger)
        {
            _dbContext = dbContext;
            _productServices = productServices;
            _logger = logger;
        }

        public async Task<BaseResponse<Guid>> CreateOrder(CreateOrder request)
        {
            _logger.LogInformation("CreateOrder method called.");

            try
            {
                if (request != null)
                {
                    // Retrieve the product from the database
                    var product = await _dbContext.Products.FindAsync(request.ProductId);
                    if (product == null)
                    {
                        _logger.LogWarning("Product not found for ProductId: {ProductId}", request.ProductId);
                        return new BaseResponse<Guid>
                        {
                            Success = false,
                            Message = "Product not found",
                            Hasherror = true
                        };
                    }

                    var order = new Order
                    {
                        ProductId = request.ProductId,
                        OrderDate = DateTime.Now,
                        TotalAmount = product.Price
                    };
                    _dbContext.Orders.Add(order);
                }

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    _logger.LogInformation("Order placed successfully.");
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = "Order has been placed successfully",
                    };
                }
                else
                {
                    _logger.LogWarning("Order placement failed.");
                    return new BaseResponse<Guid>
                    {
                        Message = "Order failed"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Order creation failed.");
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Order failed, unable to create order",
                    Hasherror = true
                };
            }
        }

        public List<SelectProductDto> GetProductSelect()
        {
            _logger.LogInformation("GetProductSelect method called.");
            var products = _dbContext.Products.ToList();
            var result = new List<SelectProductDto>();

            if (products.Count > 0)
            {
                result = products.Select(x => new SelectProductDto()
                {
                    Id = x.Id,
                    ProductName = x.Name,
                }).ToList();
            }

            _logger.LogInformation("{ProductCount} products retrieved for selection.", result.Count);
            return result;
        }

        public async Task<BaseResponse<Guid>> DeleteOrderAsync(Guid Id)
        {
            _logger.LogInformation("DeleteOrderAsync method called for OrderId: {OrderId}", Id);

            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == Id);
                if (order != null)
                {
                    _dbContext.Orders.Remove(order);
                }
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    _logger.LogInformation("Order {OrderId} deleted successfully.", Id);
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = $"Order has been deleted successfully",
                    };
                }
                else
                {
                    _logger.LogWarning("Failed to delete order {OrderId}. The order may not exist or there was an error in the deletion process.", Id);
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Failed to delete Order. The order may not exist or there was an error in the deletion process.",
                        Hasherror = true
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete order {OrderId}.", Id);
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Failed to delete Order. The order may not exist or there was an error in the deletion process.",
                    Hasherror = true
                };
            }
        }

        public async Task<List<OrderDto>> GetOrder()
        {
            _logger.LogInformation("GetOrder method called.");

            var orders = await _dbContext.Orders
                .Select(x => new OrderDto()
                {
                    Id = x.Id,
                    OrderDate = x.OrderDate,
                    TotalAmount = x.TotalAmount,
                    ProductName = x.Products.Name,
                })
                .ToListAsync();

            _logger.LogInformation("{OrderCount} orders retrieved.", orders.Count);
            return orders;
        }

        public async Task<BaseResponse<OrderDto>> GetOrderByIdAsync(Guid Id)
        {
            _logger.LogInformation("GetOrderByIdAsync method called for OrderId: {OrderId}", Id);

            var order = await _dbContext.Orders
                .Where(x => x.Id == Id)
                .Select(x => new OrderDto
                {
                    Id = x.Id,
                    ProductName = x.Products.Name,
                    OrderDate = x.OrderDate,
                    TotalAmount = x.TotalAmount,
                }).FirstOrDefaultAsync();

            if (order != null)
            {
                _logger.LogInformation("Order {OrderId} retrieved successfully.", Id);
                return new BaseResponse<OrderDto>
                {
                    Success = true,
                    Message = $"Order {Id} retrieved successfully",
                    Data = order
                };
            }
            else
            {
                _logger.LogWarning("Failed to retrieve order {OrderId}.", Id);
                return new BaseResponse<OrderDto>
                {
                    Success = false,
                    Message = $"Order {Id} retrieval failed"
                };
            }
        }

        public async Task<BaseResponse<OrderDto>> GetOrderAsync(Guid Id)
        {
            _logger.LogInformation("GetOrderAsync method called for OrderId: {OrderId}", Id);

            var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == Id);
            if (order != null)
            {
                _logger.LogInformation("Order {OrderId} retrieved successfully.", Id);
                return new BaseResponse<OrderDto>
                {
                    Message = "",
                    Success = true,
                    Data = new OrderDto
                    {
                        OrderDate = order.OrderDate,
                        TotalAmount = order.TotalAmount
                    }
                };
            }
            else
            {
                _logger.LogWarning("Failed to retrieve order {OrderId}.", Id);
                return new BaseResponse<OrderDto>
                {
                    Success = false,
                    Message = "",
                };
            }
        }

        public async Task<BaseResponse<IList<OrderDto>>> GetAllOrderAsync()
        {
            _logger.LogInformation("GetAllOrderAsync method called.");

            var orders = await _dbContext.Orders
                .Select(x => new OrderDto
                {
                    OrderDate = x.OrderDate,
                    TotalAmount = x.TotalAmount
                }).ToListAsync();

            if (orders != null)
            {
                _logger.LogInformation("{OrderCount} orders retrieved successfully.", orders.Count);
                return new BaseResponse<IList<OrderDto>>
                {
                    Success = true,
                    Message = "Orders retrieved successfully",
                    Data = orders
                };
            }
            else
            {
                _logger.LogWarning("Failed to retrieve orders.");
                return new BaseResponse<IList<OrderDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve orders. There was an error in the retrieval process",
                    Hasherror = true
                };
            }
        }

        public async Task<BaseResponse<IList<OrderDto>>> UpdateOrder(Guid Id, UpdateOrder request)
        {
            _logger.LogInformation("UpdateOrder method called for OrderId: {OrderId}", Id);

            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == Id);
                if (order == null)
                {
                    _logger.LogWarning("Order {OrderId} not found for update.", Id);
                    return new BaseResponse<IList<OrderDto>>
                    {
                        Success = false,
                        Message = $"Order with ID {Id} not found.",
                        Hasherror = true
                    };
                }

                order.OrderDate = request.OrderDate;
                order.TotalAmount = request.TotalAmount;
                _dbContext.Orders.Update(order);

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    _logger.LogInformation("Order {OrderId} updated successfully.", Id);
                    return new BaseResponse<IList<OrderDto>>
                    {
                        Success = true,
                        Message = $"Order with ID {Id} updated successfully."
                    };
                }
                else
                {
                    _logger.LogWarning("Failed to update order {OrderId}.", Id);
                    return new BaseResponse<IList<OrderDto>>
                    {
                        Success = false,
                        Message = $"Failed to update order. There was an error in the updating process.",
                        Hasherror = true
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update order {OrderId}.", Id);
                return new BaseResponse<IList<OrderDto>>
                {
                    Success = false,
                    Message = $"Failed to update order. There was an error in the updating process.",
                    Hasherror = true
                };
            }
        }
    }
}
