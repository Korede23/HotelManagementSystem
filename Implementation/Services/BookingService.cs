using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Model.Entity.Enum;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Implementation.Services
{
    public class BookingService : IBookingServices
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICustomerServices _customerServices;
        private readonly ILogger<BookingService> _logger;

        public BookingService(ApplicationDbContext dbContext, ICustomerServices customerServices, ILogger<BookingService> logger)
        {
            _dbContext = dbContext;
            _customerServices = customerServices;
            _logger = logger;
        }

        public async Task<BaseResponse<Guid>> CreateBooking(CreateBooking request)
        {
            _logger.LogInformation("CreateBooking method called.");
            try
            {
                var room = await _dbContext.Rooms.FindAsync(request.RoomId);
                if (room == null)
                {
                    _logger.LogWarning("Room not found for RoomId: {RoomId}", request.RoomId);
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Room not found",
                        Hasherror = true
                    };
                }

                var existingBooking = await _dbContext.Bookings
                                                        .Where(b => b.RoomId == request.RoomId)
                                                        .FirstOrDefaultAsync();

                if (existingBooking != null)
                {
                    _logger.LogWarning("Room is already booked for RoomId: {RoomId}", request.RoomId);
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Room is not available at the moment.",
                        Hasherror = true
                    };
                }

                var booking = new Booking()
                {
                    RoomId = request.RoomId,
                    CustomerId = request.UserId,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    TotalCost = room.RoomRate,
                };

                _dbContext.Bookings.Add(booking);

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    room.Availability = RoomAvailability.NotAvailable;
                    _dbContext.Rooms.Update(room);
                    await _dbContext.SaveChangesAsync();

                    _logger.LogInformation("Booking created successfully with Id: {BookingId}", booking.Id);
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = "Your booking has been successful",
                        Data = booking.Id
                    };
                }
                else
                {
                    _logger.LogWarning("Booking creation failed for RoomId: {RoomId}", request.RoomId);
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Booking Failed, unable to complete your booking request.",
                        Hasherror = true
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the booking for RoomId: {RoomId}", request.RoomId);
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Booking Failed, unable to complete your booking request.",
                    Hasherror = true
                };
            }
        }

        public async Task<List<BookingDto>> GetBooking()
        {
            _logger.LogInformation("GetBooking method called.");
            var bookings = await _dbContext.Bookings
                .Select(x => new BookingDto()
                {
                    Id = x.Id,
                    RoomId = x.RoomId,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    TotalCost = x.TotalCost,
                    RoomName = x.Rooms.RoomName,
                }).ToListAsync();
            _logger.LogInformation("Retrieved {Count} bookings.", bookings.Count);
            return bookings;
        }

        public async Task<BaseResponse<Guid>> DeleteBookingAsync(Guid Id)
        {
            _logger.LogInformation("DeleteBookingAsync method called with Id: {Id}", Id);
            try
            {
                var booking = await _dbContext.Bookings.FirstOrDefaultAsync(x => x.Id == Id);
                if (booking != null)
                {
                    _dbContext.Bookings.Remove(booking);
                }
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    _logger.LogInformation("Booking with Id: {Id} deleted successfully.", Id);
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = $"Booking {Id} has been deleted successfully"
                    };
                }
                else
                {
                    _logger.LogWarning("Failed to delete booking with Id: {Id}", Id);
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = $"Delete failed, unable to process the deletion of booking {Id} at this time",
                        Hasherror = true
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the booking with Id: {Id}", Id);
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = $"Delete failed, unable to process the deletion of booking {Id} at this time",
                    Hasherror = true
                };
            }
        }

        public List<SelectRoomDto> GetRoomSelect()
        {
            _logger.LogInformation("GetRoomSelect method called.");
            var roomNames = _dbContext.Rooms.ToList();
            var result = new List<SelectRoomDto>();

            if (roomNames.Count > 0)
            {
                result = roomNames.Select(x => new SelectRoomDto()
                {
                    Id = x.Id,
                    RoomName = x.RoomName,
                }).ToList();
            }

            _logger.LogInformation("Retrieved {Count} room names.", result.Count);
            return result;
        }

        public async Task<BaseResponse<BookingDto>> GetBookingAsync(Guid Id)
        {
            _logger.LogInformation("GetBookingAsync method called with Id: {Id}", Id);
            var booking = await _dbContext.Bookings.FirstOrDefaultAsync(x => x.Id == Id);
            if (booking != null)
            {
                _logger.LogInformation("Booking retrieved successfully for Id: {Id}", Id);
                return new BaseResponse<BookingDto>
                {
                    Message = "",
                    Success = true,
                    Data = new BookingDto
                    {
                        TotalCost = booking.TotalCost,
                        RoomId = booking.RoomId,
                        Email = booking.Email,
                        PhoneNumber = booking.PhoneNumber,
                        RoomName = booking.Rooms.RoomName,
                    }
                };
            }
            _logger.LogWarning("Booking not found for Id: {Id}", Id);
            return new BaseResponse<BookingDto>
            {
                Success = false,
                Message = "",
            };
        }

        public async Task<BaseResponse<BookingDto>> GetBookingByIdAsync(Guid Id)
        {
            _logger.LogInformation("GetBookingByIdAsync method called with Id: {Id}", Id);
            var bookings = await _dbContext.Bookings
             .Where(x => x.Id == Id)
             .Select(x => new BookingDto()
             {
                 Id = x.Id,
                 Rooms = x.Rooms,
                 RoomId = x.RoomId,
                 Email = x.Email,
                 PhoneNumber = x.PhoneNumber,
                 TotalCost = x.TotalCost,
             }).FirstOrDefaultAsync();
            if (bookings != null)
            {
                _logger.LogInformation("Booking retrieved successfully for Id: {Id}", Id);
                return new BaseResponse<BookingDto>
                {
                    Success = true,
                    Message = "Bookings retrieved successfully",
                    Data = bookings
                };
            }
            else
            {
                _logger.LogWarning("Failed to retrieve booking for Id: {Id}", Id);
                return new BaseResponse<BookingDto>
                {
                    Success = false,
                    Message = "Booking retrieval failed",
                    Hasherror = true
                };
            }
        }

        public async Task<BaseResponse<IList<BookingDto>>> GetAllBookingsAsync()
        {
            _logger.LogInformation("GetAllBookingsAsync method called.");
            var bookings = await _dbContext.Bookings
            .Select(x => new BookingDto()
            {
                TotalCost = x.TotalCost,
                RoomId = x.RoomId,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
            }).ToListAsync();
            if (bookings != null)
            {
                _logger.LogInformation("Retrieved {Count} bookings.", bookings.Count);
                return new BaseResponse<IList<BookingDto>>
                {
                    Success = true,
                    Message = "Bookings retrieved successfully",
                    Data = bookings
                };
            }
            else
            {
                _logger.LogWarning("Failed to retrieve bookings.");
                return new BaseResponse<IList<BookingDto>>
                {
                    Success = false,
                    Message = "Bookings retrieval failed",
                    Data = bookings,
                    Hasherror = true
                };
            }
        }

        public async Task<BaseResponse<BookingDto>> UpdateBooking(Guid Id, UpdateBooking request)
        {
            _logger.LogInformation("UpdateBooking method called with Id: {Id}", Id);
            try
            {
                var booking = await _dbContext.Bookings.FirstOrDefaultAsync(x => x.Id == Id);
                if (booking == null)
                {
                    _logger.LogWarning("Booking not found for Id: {Id}", Id);
                    return new BaseResponse<BookingDto>
                    {
                        Success = false,
                        Message = $"Booking {Id} not found",
                        Hasherror = true
                    };
                }

                var room = await _dbContext.Rooms.FindAsync(request.RoomId);
                if (room == null)
                {
                    _logger.LogWarning("Room not found for RoomId: {RoomId}", request.RoomId);
                    return new BaseResponse<BookingDto>
                    {
                        Success = false,
                        Message = "Room not found",
                        Hasherror = true
                    };
                }

                booking.TotalCost = room.RoomRate;
                booking.RoomId = request.RoomId;
                booking.PhoneNumber = request.PhoneNumber;
                booking.Email = request.Email;
                _dbContext.Bookings.Update(booking);

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    _logger.LogInformation("Booking with Id: {Id} updated successfully.", Id);
                    return new BaseResponse<BookingDto>
                    {
                        Success = true,
                        Message = $"Booking {Id} updated successfully"
                    };
                }
                else
                {
                    _logger.LogWarning("Failed to update booking with Id: {Id}", Id);
                    return new BaseResponse<BookingDto>
                    {
                        Success = false,
                        Message = $"Booking {Id} update failed",
                        Hasherror = true
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the booking with Id: {Id}", Id);
                return new BaseResponse<BookingDto>
                {
                    Success = false,
                    Message = $"Booking {Id} update failed",
                    Hasherror = true
                };
            }
        }

        public async Task<IEnumerable<ActiveBookingDto>> GetActiveBookings()
        {
            return await _dbContext.Bookings
                    .Where(b => !_dbContext.CustomerStatuses.Any(cs => cs.BookingId == b.Id))
                    .Select(b => new ActiveBookingDto
                    {
                        BookingId = b.Id,
                        CustomerId = b.CustomerId,
                        CustomerName = b.Customer.FullName
                    })
                    .ToListAsync();

        }

    }
}
