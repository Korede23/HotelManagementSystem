﻿using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;

namespace HotelManagementSystem.Implementation.Interface
{
    public interface IOrderServices
    {
        Task<BaseResponse<Guid>> CreateOrder(CreateOrder request);
        Task<BaseResponse<Guid>> DeleteOrderAsync(int Id);
        Task<BaseResponse<IList<OrderDto>>> GetOrderByIdAsync(int Id);
        Task<BaseResponse<OrderDto>> GetOrderAsync(int Id);
        Task<BaseResponse<IList<OrderDto>>> GetAllOrderAsync();
        Task<BaseResponse<IList<OrderDto>>> UpdateOrder(int Id, UpdateOrder request);
        Task<List<OrderDto>> GetOrder();
    }
}
