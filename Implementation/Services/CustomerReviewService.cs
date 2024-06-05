using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Model.Entity.Enum;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Implementation.Services
{
    public class CustomerReviewService : ICustomerReviewService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICustomerServices _customerServices;

        public CustomerReviewService(ApplicationDbContext dbContext, ICustomerServices customerServices)
        {
            _dbContext = dbContext;
            _customerServices = customerServices;
        }

        public async Task<BaseResponse<Guid>> CreateReview(CreateReview request, Guid Id)
        {
            var customer = await _customerServices.GetCustomerByIdAsync(Id);
            if (customer == null)
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "User Not Found",
                    Hasherror = true
                };
            }
            var review = new CustomerReview
            {
                // CustomerId = request.Id,
                Comment = request.Comment,


            };
            _dbContext.CustomerReviews.Add(review);
            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return new BaseResponse<Guid>
                {
                    Success = true,
                    Message = "Succesfully Commented on Review"
                };
            }
            else
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Comment Failed"
                };
            }

        }

        public async Task<List<CustomerReviewDto>> GetReview()
        {
            return _dbContext.CustomerReviews
                //.Include(x => x.Id)
                //.Include(x => x.RoomType)
                .Select(x => new CustomerReviewDto()
                {
                    Comment = x.Comment,
                    Rating = x.Rating


                }).ToList();
        }









        //public async Task<ReviewResponseDto> GetAllReviewAsync()
        //{
        //    var review = await _dbContext.CustomerReviews
        //       // .Where(x => x.)
        //       .Select(x => new CustomerReviewDto
        //       {

        //           Comment = x.Comment

        //       }).ToListAsync();

        //    if (review != null)
        //    {
        //        return new ReviewResponseDto
        //        {
        //            Success = true,
        //            Message = "Review Retrieved Succesfully",
        //            Data = review
        //        };
        //    }
        //    else
        //    {
        //        return new ReviewResponseDto
        //        {
        //            Success = false,
        //            Message = "failed",
        //            Hasherror = true
        //        };
        //    }
        //}



        public async Task<BaseResponse<IList<CustomerReviewDto>>> GetAllReviewAsync()
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

            if (review != null )
            {
                return new BaseResponse<IList<CustomerReviewDto>>
                {
                    Success = true,
                    Message = "Review Retrieved Successfully",
                    Data = review
                };
            }
            else
            {
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

