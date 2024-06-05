using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;

namespace HotelManagementSystem.Implementation.Interface
{
    public interface ICustomerServices
    {
        Task<BaseResponse<Guid>> CreateCustomer(CreateCustomer request);
        Task<BaseResponse<Guid>> DeleteCustomerAsync(Guid Id);
        Task<BaseResponse<IList<CustomerDto>>> GetCustomerByIdAsync(Guid Id);
        Task<BaseResponse<CustomerDto>> GetCustomerAsync(Guid Id);
        Task<BaseResponse<IList<CustomerDto>>> GetAllCustomerCreatedAsync();
        Task<BaseResponse<CustomerDto>> UpdateCustomer(Guid Id, UpdateCustomer request);
        Task<List<CustomerDto>> GetCustomer();
    }
}
