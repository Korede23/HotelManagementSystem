using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace HMS.Implementation.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IProductServices _productServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<OrderServices> _logger;
        private readonly string _secretKey;
        private readonly HttpClient _httpClient;

        public OrderServices(ApplicationDbContext dbContext, IProductServices productServices,
            IHttpContextAccessor httpContextAccessor, UserManager<User> userManager,
            IConfiguration configuration, HttpClient httpClient,
             ILogger<OrderServices> logger)
        {
            _dbContext = dbContext;
            _productServices = productServices;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _secretKey = configuration["Paystack:SecretKey"];
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<BaseResponse<InitializePaymentResponseDto>> CreateOrder(CreateOrder request)
        {
            _logger.LogInformation("CreateOrder method called.");

            var userPrincipal = _httpContextAccessor.HttpContext?.User;
            if (userPrincipal == null)
            {
                return new BaseResponse<InitializePaymentResponseDto>
                {
                    Success = false,
                    Message = "User not authenticated"
                };
            }

            var user = await _userManager.GetUserAsync(userPrincipal);
            if (user == null)
            {
                return new BaseResponse<InitializePaymentResponseDto>
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var product = await _dbContext.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                return new BaseResponse<InitializePaymentResponseDto>
                {
                    Success = false,
                    Message = "Product not found"
                };
            }

            var order = new Order
            {
                ProductId = request.ProductId,
                OrderDate = DateTime.Now,
                TotalAmount = product.Price,
                CreatedBy = user.UserName
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = order.TotalAmount,
                Email = user.Email,
                Status = "Pending",
                TransactionReference = Guid.NewGuid().ToString(),
                DateRequested = DateTime.Now,
                CreatedBy = user.UserName

            };

            _dbContext.Payments.Add(payment);
            await _dbContext.SaveChangesAsync();

            try
            {
                string callbackUrl = "https://localhost:7211/call-back-url";
                var requestPayload = new
                {
                    amount = order.TotalAmount * 100,
                    email = user.Email.Trim(),
                    reference = payment.TransactionReference,
                    callback_url = callbackUrl
                };

                var requestBody = new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json");

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.paystack.co/transaction/initialize")
                {
                    Headers = { Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _secretKey) },
                    Content = requestBody
                };

                var response = await _httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var paystackResponse = JsonConvert.DeserializeObject<PaystackResponseDto<InitializePaymentResponseDto>>(responseContent);

                    if (paystackResponse != null && paystackResponse.Status)
                    {
                        payment.Status = "Initialized";
                        await _dbContext.SaveChangesAsync();

                        return new BaseResponse<InitializePaymentResponseDto>
                        {
                            Success = true,
                            Message = "Payment initialization successful",
                            Data = paystackResponse.Data
                        };
                    }
                    else
                    {
                        return new BaseResponse<InitializePaymentResponseDto>
                        {
                            Success = false,
                            Message = $"Payment initialization failed. Response: {paystackResponse?.Message ?? "Unknown error"}",
                        };
                    }
                }
                else
                {
                    return new BaseResponse<InitializePaymentResponseDto>
                    {
                        Success = false,
                        Message = $"Payment initialization failed. Status Code: {response.StatusCode}. Response: {responseContent}",
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<InitializePaymentResponseDto>
                {
                    Success = false,
                    Message = $"An error occurred while initializing payment: {ex.Message}",
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

        public async Task<List<OrderDto>> GetOrders()
        {
            _logger.LogInformation("GetOrders method called.");

            var userPrincipal = _httpContextAccessor.HttpContext?.User;
            if (userPrincipal == null)
            {
                return new List<OrderDto>();
            }

            var user = await _userManager.GetUserAsync(userPrincipal);
            if (user == null)
            {
                return new List<OrderDto>();
            }

            var orders = await _dbContext.Orders
                .Where(o => o.CreatedBy == user.UserName)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    ProductName = o.Products.Name
                })
                .ToListAsync();

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
