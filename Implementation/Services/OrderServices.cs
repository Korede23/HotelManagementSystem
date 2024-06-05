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

        public OrderServices(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponse<Guid>> CreateOrder(CreateOrder request)
        {
            try
            {
                if (request != null)
                {
                    var order = new Order
                    {
                        CustomerId = request.CustomerId,
                        OrderDate = request.OrderDate,
                        ProductId = request.ProductId,
                        TotalAmount = request.TotalAmount

                    };
                    _dbContext.Orders.Add(order);
                }

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = "Order  has been Placed Succesfully"
                    };
                }
                else
                {
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Order Failed"

                    };
                }


            }
            catch (Exception ex)
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Order failed ,Unable to create Order",
                    Hasherror = true
                };
            }
        }


        public async Task<BaseResponse<Guid>> DeleteOrderAsync(Guid Id)
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

        public async Task<List<OrderDto>> GetOrder()
        {
            return await _dbContext.Orders
                .Select(x => new OrderDto()
                {
                    CustomerId = x.CustomerId,
                    OrderDate = x.OrderDate,
                    ProductId = x.ProductId,
                    TotalAmount = x.TotalAmount
                })
                .ToListAsync();
        }

        public async Task<BaseResponse<IList<OrderDto>>> GetOrderByIdAsync(Guid Id)
        {
            var order = await _dbContext.Orders
            .Where(x => x.Id == Id)
            .Select(x => new OrderDto
            {
                CustomerId = x.CustomerId,
                OrderDate = x.OrderDate,
                Product = x.Product,
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
                        CustomerId = order.CustomerId,
                        OrderDate = order.OrderDate,
                        Product = order.Product,
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
               CustomerId = x.CustomerId,
               OrderDate = x.OrderDate,
               Product = x.Product,
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
            var order = await _dbContext.Orders.FirstOrDefaultAsync();
            if (order != null)
            {
                order.CustomerId = request.CustomerId;
                order.OrderDate = request.OrderDate;
                order.Product = request.Product;
                order.TotalAmount = request.TotalAmount;
            }
            _dbContext.Orders.Add(order);
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
                    Message = $"Failed to Update order {request.CustomerId} ,there was an error in the updating process.",
                    Hasherror = true
                };
            }
        }
    }
}
