using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;

namespace HotelManagementSystem.Implementation.Interface
{
    public interface ICustomerStatusServices
    {
        Task<BaseResponse<Guid>> CheckIn(string customerId ,Guid bookingId);
        Task<List<CustomerStatusDto>> GetCustomerStatus();
        List<SelectCustomerDto> GetCustomerSelect();
        List<SelectCustomerCheckedInDto> GetSelectCustomerCheckedIn();
        Task<BaseResponse<Guid>> CheckOut(Guid customerId);
    }
}
