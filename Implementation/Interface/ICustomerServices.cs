using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;

namespace HotelManagementSystem.Implementation.Interface
{
    public interface ICustomerServices
    {
        Task<BaseResponse<Guid>> DeleteCustomerAsync(string Id);
        Task<BaseResponse<CustomerDto>> GetCustomerByIdAsync(string Id);
        Task<BaseResponse<CustomerDto>> GetCustomerAsync(string Id);
        Task<BaseResponse<IList<CustomerDto>>> GetAllCustomerCreatedAsync();
        Task<BaseResponse<CustomerDto>> UpdateCustomer(string Id, UpdateCustomer request);
        Task<List<CustomerDto>> GetCustomer();
    }
}
