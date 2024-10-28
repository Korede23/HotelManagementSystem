using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Text;
namespace HotelManagementSystem.Implementation.Services
{
    public class PaystackService : IPaystackService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBookingServices _bookingServices;
        private readonly ILogger<PaystackService> _logger;
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _dbcontext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IUserServices _userServices;
        private readonly IOrderServices _orderServices;
        private readonly string _secretKey;


        public PaystackService(ApplicationDbContext dbContext, IBookingServices bookingServices, ILogger<PaystackService> logger,
         IHttpContextAccessor httpContextAccessor,
         UserManager<User> userManager, HttpClient httpClient, ApplicationDbContext dbcontext, IConfiguration configuration, IUserServices userServices, IOrderServices orderServices)
        {
            _dbContext = dbContext;
            _bookingServices = bookingServices;
            _logger = logger;
            _httpClient = httpClient;
            _dbcontext = dbcontext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _userServices = userServices;
            _orderServices = orderServices;
            _secretKey = configuration["Paystack:SecretKey"];
        }

        public async Task<BaseResponse<InitializePaymentResponseDto>> InitializePaymentAsync(InitializePaymentRequestDto requestDto, string userId, Guid bookingId)
        {
            _logger.LogInformation("InitializePaymentRequestDto called with BookingId: {BookingId}, UserId: {UserId}, OrderId: {OrderId}", bookingId, userId);

            try
            {
                var booking = await _bookingServices.GetBookingByIdAsync(bookingId);
                var user = await _userServices.GetUserByIdAsync(userId);

                if (booking == null)
                {
                    return new BaseResponse<InitializePaymentResponseDto>
                    {
                        Message = $"Booking with ID {bookingId} not found",
                        Success = false
                    };
                }

                if (user == null)
                {
                    return new BaseResponse<InitializePaymentResponseDto>
                    {
                        Message = $"User with ID {userId} not found",
                        Success = false
                    };
                }
                var userPrincipal = _httpContextAccessor.HttpContext?.User;
                if (userPrincipal == null)
                {
                    return new BaseResponse<InitializePaymentResponseDto>
                    {
                        Success = false,
                        Message = "User not authenticated"
                    };
                }

                var userName = await _userManager.GetUserAsync(userPrincipal);
                if (user == null)
                {
                    return new BaseResponse<InitializePaymentResponseDto>
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                var payment = new Payment
                {
                    BookingId = bookingId,
                    Email = requestDto.Email,
                    Amount = booking.Data.TotalCost,
                    Status = "Pending",
                    DateRequested = DateTime.Now,
                    TransactionReference = Guid.NewGuid().ToString("N"),
                    CreatedOn = DateTime.Now,
                    CreatedBy = userName.UserName
                };

                _dbcontext.Payments.Add(payment);
                await _dbcontext.SaveChangesAsync();

                string callbackUrl = "https://localhost:7211/Payment/PaymentCallback";

                var requestPayload = new
                {
                    amount = booking.Data.TotalCost * 100,
                    email = requestDto.Email.Trim(),
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
                        await _dbcontext.SaveChangesAsync();

                        return new BaseResponse<InitializePaymentResponseDto>
                        {
                            Message = "Payment initialization successful",
                            Success = true,
                            Data = paystackResponse.Data
                        };
                    }
                    else
                    {
                        return new BaseResponse<InitializePaymentResponseDto>
                        {
                            Message = $"Payment initialization failed. Response: {paystackResponse?.Message ?? "Unknown error"}",
                            Success = false
                        };
                    }
                }
                else
                {
                    return new BaseResponse<InitializePaymentResponseDto>
                    {
                        Message = $"Payment initialization failed. Status Code: {response.StatusCode}. Response: {responseContent}",
                        Success = false
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<InitializePaymentResponseDto>
                {
                    Message = $"An error occurred while initializing payment: {ex.Message}",
                    Success = false
                };
            }
        }

        public async Task<BaseResponse<VerifyPaymentRequestDto>> VerifyPaymentAsync(string reference)
        {
            try
            {

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://api.paystack.co/transaction/verify/{reference}");
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _secretKey);

                var response = await _httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new BaseResponse<VerifyPaymentRequestDto>
                    {
                        Message = $"Payment verification failed. Status Code: {response.StatusCode}. Response: {responseContent}",
                        Success = false
                    };
                }

                var verifyResponse = JsonConvert.DeserializeObject<VerifyPaymentRequestDto>(responseContent);

                return new BaseResponse<VerifyPaymentRequestDto>
                {
                    Message = "Payment verification successful",
                    Success = true,
                    Data = verifyResponse
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<VerifyPaymentRequestDto>
                {
                    Message = $"An error occurred while verifying payment: {ex.Message}",
                    Success = false
                };
            }
        }

    }
}



