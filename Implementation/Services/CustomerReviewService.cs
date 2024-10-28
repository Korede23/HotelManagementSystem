using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Model.Entity.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace HotelManagementSystem.Implementation.Services
{
    public class CustomerReviewService : ICustomerReviewService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICustomerServices _customerServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<CustomerReviewService> _logger;

        public CustomerReviewService(ApplicationDbContext dbContext, ICustomerServices customerServices,
         IHttpContextAccessor httpContextAccessor,
         UserManager<User> userManager, ILogger<CustomerReviewService> logger)
        {
            _dbContext = dbContext;
            _customerServices = customerServices;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<BaseResponse<Guid>> CreateReview(CreateReview request, string Id)
        {
            _logger.LogInformation("CreateReview called with customer Id: {Id}", Id);
            try
            {
                var customer = await _customerServices.GetCustomerByIdAsync(Id);
                if (customer == null)
                {
                    _logger.LogWarning("Customer not found with Id: {Id}", Id);
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "User Not Found",
                        Hasherror = true
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
                var review = new CustomerReview
                {
                    Comment = request.Comment,
                    Rating = request.Rating,
                    CreatedBy = user.UserName
                };
                _dbContext.CustomerReviews.Add(review);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    _logger.LogInformation("Review created successfully for customer Id: {Id}", Id);
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = "Successfully Commented on Review"
                    };
                }
                else
                {
                    _logger.LogWarning("Failed to create review for customer Id: {Id}", Id);
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Comment Failed"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while creating review for customer Id: {Id}", Id);
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Comment Failed"
                };
            }
        }

        public async Task<List<CustomerReviewDto>> GetReview()
        {
            _logger.LogInformation("GetReview called");
            return await _dbContext.CustomerReviews
                .Select(x => new CustomerReviewDto()
                {
                    Comment = x.Comment,
                    Rating = x.Rating
                }).ToListAsync();
        }

        public async Task<BaseResponse<IList<CustomerReviewDto>>> GetAllReviewAsync()
        {
            _logger.LogInformation("GetAllReviewAsync called");
            try
            {
                var review = await _dbContext.CustomerReviews
                    .Where(x => (int)x.Rating == (int)Review.Excellent ||
                                (int)x.Rating == (int)Review.Good ||
                                (int)x.Rating == (int)Review.Bad)
                    .Select(x => new CustomerReviewDto
                    {
                        Comment = x.Comment,
                    })
                    .ToListAsync();

                if (review != null)
                {
                    _logger.LogInformation("Reviews retrieved successfully");
                    return new BaseResponse<IList<CustomerReviewDto>>
                    {
                        Success = true,
                        Message = "Review Retrieved Successfully",
                        Data = review
                    };
                }
                else
                {
                    _logger.LogWarning("No reviews found");
                    return new BaseResponse<IList<CustomerReviewDto>>
                    {
                        Success = false,
                        Message = "Failed to retrieve reviews",
                        Hasherror = true
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving reviews");
                return new BaseResponse<IList<CustomerReviewDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve reviews",
                    Hasherror = true
                };
            }
        }
    }
}
