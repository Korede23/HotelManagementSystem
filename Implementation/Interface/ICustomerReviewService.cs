using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;

namespace HotelManagementSystem.Implementation.Interface
{
    public interface ICustomerReviewService
    {
        Task<BaseResponse<Guid>> CreateReview(CreateReview request, Guid Id);
        Task<BaseResponse<IList<CustomerReviewDto>>> GetAllReviewAsync();
        Task<List<CustomerReviewDto>> GetReview();

    }
}
