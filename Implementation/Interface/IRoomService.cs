using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;

namespace HotelManagementSystem.Implementation.Interface
{
    public interface IRoomService
    {
        Task<BaseResponse<Guid>> CreateRoom(CreateRoom request);
        Task<List<RoomDto>> GetRoom();
        Task<BaseResponse<Guid>> DeleteRoomAsync(Guid Id);
        Task<BaseResponse<RoomDto>> GetRoomAsync(Guid Id);
        Task<BaseResponse<IList<RoomDto>>> GetAllRoomsCreatedAsync();
        Task<BaseResponse<IList<RoomDto>>> GetRoomsByIdAsync(Guid Id);
        Task<BaseResponse<RoomDto>> UpdateRoom(Guid Id, UpdateRoom request);
        Task<List<SelectAmenity>> GetAmenity();

    }
}