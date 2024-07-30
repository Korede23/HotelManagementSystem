using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;

namespace HotelManagementSystem.Implementation.Interface
{
    public interface IRoomService
    {
        Task<BaseResponse<Guid>> CreateRoom(CreateRoom request);
        Task<BaseResponse<Guid>> DeleteRoomAsync(Guid Id);
        Task<BaseResponse<RoomDto>> GetRoomAsync(Guid Id);
        Task<List<RoomDto>> GetAllRoomsCreatedAsync();
        Task<BaseResponse<RoomDto>> GetRoomsByIdAsync(Guid Id);
        Task<BaseResponse<RoomDto>> UpdateRoom(Guid Id, UpdateRoom request);
        List<SelectAmenityDto> GetAmenitySelect();

    }
}