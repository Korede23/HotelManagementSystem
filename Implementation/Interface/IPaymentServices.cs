using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;

namespace HotelManagementSystem.Implementation.Interface
{
    public interface IPaymentServices
    {
        Task<BaseResponse<Guid>> CreatePayment(CreatePayment request);
        Task<PaymentDto> GetPaymentById(Guid paymentId);
        Task<BaseResponse<IList<PaymentDto>>> GetAllPaymentAsync();
        Task<List<PaymentDto>> GetPayment();
    }
}
