using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;

namespace HotelManagementSystem.Implementation.Interface
{
    public interface IBookingServices
    {
        Task<BaseResponse<Guid>> CreateBooking(CreateBooking request, Guid Id);
        Task<BaseResponse<Guid>> DeleteBookingAsync(Guid Id);
        Task<BaseResponse<IList<BookingDto>>> GetBookingByIdAsync(Guid Id);
        Task<BaseResponse<IList<BookingDto>>> GetAllBookingsAsync();
        Task<BaseResponse<BookingDto>> UpdateBooking(Guid Id, UpdateBooking request);
        Task<BaseResponse<BookingDto>> GetBookingAsync(Guid Id);
        Task<List<BookingDto>> GetBooking();
        List<SelectRoomDto> GetRoomSelect();




    }
}
