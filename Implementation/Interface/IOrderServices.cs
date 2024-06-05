using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;

namespace HotelManagementSystem.Implementation.Interface
{
    public interface IOrderServices
    {
        Task<BaseResponse<Guid>> CreateOrder(CreateOrder request);
        Task<BaseResponse<Guid>> DeleteOrderAsync(Guid Id);
        Task<BaseResponse<IList<OrderDto>>> GetOrderByIdAsync(Guid Id);
        Task<BaseResponse<OrderDto>> GetOrderAsync(Guid Id);
        Task<BaseResponse<IList<OrderDto>>> GetAllOrderAsync();
        Task<BaseResponse<IList<OrderDto>>> UpdateOrder(Guid Id, UpdateOrder request);
        Task<List<OrderDto>> GetOrder();
    }
}
