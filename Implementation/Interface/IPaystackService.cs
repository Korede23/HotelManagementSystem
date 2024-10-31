using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;

namespace HotelManagementSystem.Implementation.Interface
{
    public interface IPaystackService
    {
        Task<BaseResponse<InitializePaymentResponseDto>> InitializePaymentAsync(InitializePaymentRequestDto requestDto, string userId, Guid bookingIdb);
        Task<BaseResponse<VerifyPaymentRequestDto>> VerifyPaymentAsync(string reference);
        Task<PaginatedResponse<List<PaymentDto>>> GetAllPayments(int pageNumber, int pageSize);
        Task<PaymentDto> GetPaymentById(Guid Id);
    }
}
