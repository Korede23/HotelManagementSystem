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
        Task<PaginatedResponse<List<RoomDto>>> GetRoomsCreatedAsync(int pageNumber, int pageSize);
        Task<BaseResponse<RoomDto>> GetRoomsByIdAsync(Guid Id);
        Task<BaseResponse<RoomDto>> UpdateRoom(Guid Id, UpdateRoom request);
        List<SelectAmenityDto> GetAmenitySelect();
        Task<PaginatedResponse<List<RoomDto>>> GetAllRoomsCreatedAsync(int pageNumber, int pageSize, string searchAmenity, string available, decimal roomRate, string searchTerm = null);

    }
}