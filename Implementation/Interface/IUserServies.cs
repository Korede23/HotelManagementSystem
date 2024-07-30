using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;

namespace HotelManagementSystem.Implementation.Interface
{
    public interface IUserServices
    {
        Task<BaseResponse<Guid>> CreateUser(CreateUser request);
        Task<BaseResponse<Guid>> DeleteUserAsync(string id);
         Task<BaseResponse<UserDto>> GetUserAsync(string Id);
        Task<BaseResponse<UserDto>> GetUserByIdAsync(string id);
        Task<BaseResponse<IList<UserDto>>> GetAllUserAsync();
        Task<List<UserDto>> GetUser();
        Task<Status> LoginAsync(LoginModel loginModel);
        Task LogOutAsync();
        Task<Status> ChangePasswordAsync(ChangePasswordModel changePasswordModel,string UserName);
        Task<BaseResponse<IList<UserDto>>> UpdateUser(string Id, UpdateUser request);
    }
}
