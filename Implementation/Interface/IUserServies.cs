﻿using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;

namespace HotelManagementSystem.Implementation.Interface
{
    public interface IUserServices
    {
        Task<BaseResponse<Guid>> CreateUser(CreateUser request);
        Task<BaseResponse<Guid>> DeleteUserAsync(Guid id);
        Task<BaseResponse<UserDto>> GetUserByIdAsync(Guid Id);
        Task<BaseResponse<UserDto>> GetUserAsync(Guid Id);
        Task<BaseResponse<IList<UserDto>>> GetAllUserAsync();
        Task<BaseResponse<IList<UserDto>>> UpdateUser(Guid Id, UpdateUser request);
        Task<List<UserDto>> GetUser();
        Task<Status> LoginAsync(LoginModel loginModel);
        Task LogOutAsync();
        Task<Status> ChangePasswordAsync(ChangePasswordModel changePasswordModel,string username);
    }
}
