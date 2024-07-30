using HotelManagementSystem.Dto;
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

        public OrderServices(ApplicationDbContext dbContext, IProductServices productServices)
        {
            _dbContext = dbContext;
            _productServices = productServices;
        }

        //public async Task<BaseResponse<Guid>> CreateOrder(CreateOrder request)
        //{
        //    try
        //    {
        //        if (request != null)
        //        {
        //            var order = new Order
        //            {
        //                Product = request.Product,
        //                OrderDate = request.OrderDate,
        //                TotalAmount = request.TotalAmount

        //            };
        //            _dbContext.Orders.Add(order);
        //        }

        //        if (await _dbContext.SaveChangesAsync() > 0)
        //        {
        //            return new BaseResponse<Guid>
        //            {
        //                Success = true,
        //                Message = "Order  has been Placed Succesfully"
        //            };
        //        }
        //        else
        //        {
        //            return new BaseResponse<Guid>
        //            {
        //                Message = "Order Failed"

        //            };
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return new BaseResponse<Guid>
        //        {
        //            Success = false,
        //            Message = "Order failed ,Unable to create Order",
        //            Hasherror = true
        //        };
        //    }
        //}

        public async Task<BaseResponse<Guid>> CreateOrder(CreateOrder request)
        {
            try
            {
                if (request != null)
                {
                    // Retrieve the product from the database
                    var product = await _dbContext.Products.FindAsync(request.ProductId);
                    if (product == null)
                    {
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
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = "Order has been placed successfully",
                        //Data = order.Id
                    };
                }
                else
                {
                    return new BaseResponse<Guid>
                    {
                        Message = "Order failed"
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Order failed, unable to create order",
                    Hasherror = true
                };
            }
        }


        // Select product
        public List<SelectProductDto> GetProductSelect()
        {
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

            return result;
        }



        public async Task<BaseResponse<Guid>> DeleteOrderAsync(Guid Id)
        {
            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == Id);
                if (order != null)
                {
                    _dbContext.Orders.Remove(order);
                }
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = $"Order Has been deleted succesfully",
                    };
                }

                else
                {
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = " Failed to delete Order,The order may not exist or there was an error in the deletion process.",
                        Hasherror = true
                    };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = " Failed to delete Order,The order may not exist or there was an error in the deletion process.",
                    Hasherror = true
                };
            }

        }

        public async Task<List<OrderDto>> GetOrder()
        {
            return await _dbContext.Orders
                .Select(x => new OrderDto()
                {
                    Id = x.Id,
                    OrderDate = x.OrderDate,
                    TotalAmount = x.TotalAmount,
                    ProductName = x.Products.Name,
                })
                .ToListAsync();
        }



        public async Task<BaseResponse<OrderDto>> GetOrderByIdAsync(Guid Id)
        {
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
                return new BaseResponse<OrderDto>
                {
                    Success = true,
                    Message = $"Order {Id} Retrieved successfully",
                    Data = order
                };
            }
            else
            {
                return new BaseResponse<OrderDto>
                {
                    Success = false,
                    Message = $"Order {Id} Retrieval Failed"
                };
            }
        }





        public async Task<BaseResponse<OrderDto>> GetOrderAsync(Guid Id)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == Id);
            if (order != null)
            {
                return new BaseResponse<OrderDto>
                {
                    Message = "",
                    Success = true,
                    Data = new OrderDto
                    {

                        OrderDate = order.OrderDate,
                        //Product = order.Product,
                        TotalAmount = order.TotalAmount
                    }

                };
            }
            return new BaseResponse<OrderDto>
            {
                Success = false,
                Message = "",
            };
        }


        public async Task<BaseResponse<IList<OrderDto>>> GetAllOrderAsync()
        {
            var order = await _dbContext.Orders
           .Select(x => new OrderDto
           {

               OrderDate = x.OrderDate,
               // Product = x.Product,
               TotalAmount = x.TotalAmount
           }).ToListAsync();

            if (order != null)
            {
                return new BaseResponse<IList<OrderDto>>
                {
                    Success = true,
                    Message = "Order Retrieved Succesfully",
                    Data = order
                };
            }
            else
            {
                return new BaseResponse<IList<OrderDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve order , there was an error in the retrieval process",
                    Hasherror = true
                };
            }

        }


        public async Task<BaseResponse<IList<OrderDto>>> UpdateOrder(Guid Id, UpdateOrder request)
        {
            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync();
                if (order == null)
                {
                    return new BaseResponse<IList<OrderDto>>
                    {
                        Success = true,
                        Message = $"Order with ID {Id} Updated successfully.",

                    };
                }
                order.OrderDate = request.OrderDate;
                //order.Product = request.Product;
                order.TotalAmount = request.TotalAmount;
                _dbContext.Orders.Update(order);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return new BaseResponse<IList<OrderDto>>
                    {
                        Success = true,
                        Message = $"Order with ID {Id} Updated successfully."
                    };
                }
                else
                {
                    return new BaseResponse<IList<OrderDto>>
                    {
                        Success = false,
                        Message = $"Failed to Update order,there was an error in the updating process.",
                        Hasherror = true
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<IList<OrderDto>>
                {
                    Success = false,
                    Message = $"Failed to Update order,there was an error in the updating process.",
                    Hasherror = true
                };
            }

        }
    }
}
