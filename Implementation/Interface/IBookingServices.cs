using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Model.Entity;

namespace HotelManagementSystem.Implementation.Interface
{
    public interface IBookingServices
    {
        Task<BaseResponse<Guid>> CreateBooking(CreateBooking request);
        Task<BaseResponse<Guid>> DeleteBookingAsync(Guid Id);
        Task<BaseResponse<BookingDto>> GetBookingByIdAsync(Guid Id);
        Task<BaseResponse<IList<BookingDto>>> GetAllBookingsAsync();
        Task<BaseResponse<BookingDto>> UpdateBooking(Guid Id, UpdateBooking request);
        Task<BaseResponse<BookingDto>> GetBookingAsync(Guid Id);
        List<SelectRoomDto> GetRoomSelect();
        Task<IEnumerable<ActiveBookingDto>> GetActiveBookings();
        Task<PaginatedResponse<List<BookingDto>>> GetBooking(int pageNumber = 1, int pageSize = 5);
    }
}
